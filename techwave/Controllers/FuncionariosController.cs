using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using techwave.Models;
using techwave.Data;

namespace techwave.Controllers
{
    public class FuncionariosController : Controller
    {
        private readonly techwaveContext _context;

        public FuncionariosController(techwaveContext context)
        {
            _context = context;
        }

        // GET: Funcionarios
        public async Task<IActionResult> Index()
        {
            var funcionarios = await _context.Funcionario
                .Include(f => f.Endereco)
                .Include(f => f.Usuario)
                .ToListAsync();

            return View(funcionarios);
        }

        // GET: Funcionarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Funcionario == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionario
                .Include(f => f.Endereco)
                .Include(f => f.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcionario == null)
            {
                return NotFound();
            }

            return View(funcionario);
        }

        // GET: Funcionarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Funcionarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Telefone,Endereco,Usuario")] Funcionario funcionario)
        {
            Console.WriteLine("Método Create chamado");
            if (ModelState.IsValid)
            {
                Console.WriteLine("ModelState válido");
                _context.Endereco.Add(funcionario.Endereco);
                await _context.SaveChangesAsync();
                _context.Usuario.Add(funcionario.Usuario);
                await _context.SaveChangesAsync();
                funcionario.EnderecoId = funcionario.Endereco.Id;
                funcionario.UsuarioId = funcionario.Usuario.Id;
                _context.Funcionario.Add(funcionario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            Console.WriteLine("ModelState inválido");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return View(funcionario);
        }


        // GET: Funcionarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Funcionario == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionario.Include(c => c.Endereco).Include(c => c.Usuario).FirstOrDefaultAsync(m => m.Id == id);
            if (funcionario == null)
            {
                return NotFound();
            }
            ViewData["EnderecoId"] = new SelectList(_context.Endereco, "Id", "CEP", funcionario.EnderecoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Email", funcionario.Usuario.Id);
            return View(funcionario);
        }

        // POST: Funcionarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Telefone,Endereco,Usuario")] Funcionario funcionario)
        {
            if (id != funcionario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(funcionario.Endereco);
                    await _context.SaveChangesAsync();

                    _context.Update(funcionario.Usuario);
                    await _context.SaveChangesAsync();

                    _context.Update(funcionario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FuncionarioExists(funcionario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnderecoId"] = new SelectList(_context.Endereco, "Id", "CEP", funcionario.EnderecoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Email", funcionario.UsuarioId);
            return View(funcionario);
        }

        // GET: Funcionarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Funcionario == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionario
               .Include(c => c.Endereco)
               .Include(c => c.Usuario)
               .FirstOrDefaultAsync(m => m.Id == id);

            if (funcionario == null)
            {
                return NotFound();
            }

            return View(funcionario);
        }

        // POST: Funcionarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Funcionario == null)
            {
                return Problem("Entity set 'techwaveContext.Funcionario'  is null.");
            }
            var funcionario = await _context.Funcionario.FindAsync(id);
            if (funcionario != null)
            {
                _context.Funcionario.Remove(funcionario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FuncionarioExists(int id)
        {
            return (_context.Funcionario?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

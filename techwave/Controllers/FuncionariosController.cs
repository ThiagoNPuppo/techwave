using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.ProjectModel;
using ProjetoFinal.Models;
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
            ViewData["EnderecoId"] = new SelectList(_context.Endereco, "Id", "CEP");
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Email");
            return View();
        }

        // POST: Funcionarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Telefone,Endereco,Usuario")] Funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                //adicionado o endereço
                _context.Endereco.Add(funcionario.Endereco);
                await _context.SaveChangesAsync();

                // Adicionado o usuário ao contexto e salve as mudanças
                _context.Usuario.Add(funcionario.Usuario);
                await _context.SaveChangesAsync();

                // Associado os IDs do endereço e usuário ao cliente
                funcionario.EnderecoId = funcionario.Endereco.Id;
                funcionario.UsuarioId = funcionario.Usuario.Id;

                // Adicione o cliente ao contexto e salve as mudanças
                _context.Funcionario.Add(funcionario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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

            var funcionario = await _context.Cliente.Include(c => c.Endereco).Include(c => c.Usuario).FirstOrDefaultAsync(m => m.Id == id);
            if (funcionario == null)
            {
                return NotFound();
            }
            ViewData["EnderecoId"] = new SelectList(_context.Endereco, "Id", "CEP", funcionario.EnderecoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Email", funcionario.Usuario.Id); 
            return View(funcionario);
        }

        // POST: Funcionarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    // Atualiza o endereço
                    _context.Update(funcionario.Endereco);
                    await _context.SaveChangesAsync();

                    // Atualiza o usuário
                    _context.Update(funcionario.Usuario);
                    await _context.SaveChangesAsync();

                    // Atualiza o cliente
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

            var funcionario = await _context.Cliente
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

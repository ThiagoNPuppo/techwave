using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoFinal.Models;
using techwave.Data;

namespace techwave.Controllers
{
    public class ClientesController : Controller
    {
        private readonly techwaveContext _context;

        public ClientesController(techwaveContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var clientes = await _context.Cliente
                                         .Include(c => c.Endereco)
                                         .Include(c => c.Usuario)
                                         .ToListAsync();
            return View(clientes);
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .Include(c => c.Endereco)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            ViewData["EnderecoId"] = new SelectList(_context.Endereco, "Id", "CEP");
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Email");
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Telefone,Endereco,Usuario")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                //adicionado o endereço
                _context.Endereco.Add(cliente.Endereco);
                await _context.SaveChangesAsync();

                // Adicionado o usuário ao contexto e salve as mudanças
                _context.Usuario.Add(cliente.Usuario);
                await _context.SaveChangesAsync();

                // Associado os IDs do endereço e usuário ao cliente
                cliente.EnderecoId = cliente.Endereco.Id;
                cliente.UsuarioId = cliente.Usuario.Id;

                // Adicione o cliente ao contexto e salve as mudanças
                _context.Cliente.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        //Incluir relacionamentos endereço e usuario
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente.Include(c => c.Endereco).Include(c => c.Usuario).FirstOrDefaultAsync(m => m.Id == id);  
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["EnderecoId"] = new SelectList(_context.Endereco, "Id", "CEP", cliente.EnderecoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Email", cliente.Usuario.Id);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // Atualizar e salvar no bd
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Telefone,Endereco,Usuario")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {                
                    // Atualiza o endereço
                    _context.Update(cliente.Endereco);
                    await _context.SaveChangesAsync();

                    // Atualiza o usuário
                    _context.Update(cliente.Usuario);
                    await _context.SaveChangesAsync();

                    // Atualiza o cliente
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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
            ViewData["EnderecoId"] = new SelectList(_context.Endereco, "Id", "CEP", cliente.EnderecoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Email", cliente.UsuarioId);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .Include(c => c.Endereco)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cliente == null)
            {
                return Problem("Entity set 'techwaveContext.Cliente'  is null.");
            }
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente != null)
            {
                _context.Cliente.Remove(cliente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
          return (_context.Cliente?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

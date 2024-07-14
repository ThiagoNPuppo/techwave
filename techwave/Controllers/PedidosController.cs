using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
//using techwave.Models;
using techwave.Data;
using techwave.Models;

namespace techwave.Controllers
{
    public class PedidosController : Controller
    {
        private readonly techwaveContext _context;

        public PedidosController(techwaveContext context)
        {
            _context = context;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            var pedidos = _context.Pedido.Include(p => p.Cliente).Include(p => p.PedidoProdutos).ThenInclude(pp => pp.Produto);
            return View(await pedidos.ToListAsync());
        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pedido == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido
                .Include(p => p.Cliente)
                .Include(p => p.PedidoProdutos).ThenInclude(pp => pp.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Pedidos/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Nome");
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "Id", "Nome");
            return View();
        }

        // POST: Pedidos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClienteId,DataPedido,Status,PedidoProdutos")] Pedido pedido, int[] produtoIds, int[] quantidades)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < produtoIds.Length; i++)
                {
                    pedido.PedidoProdutos.Add(new PedidoProduto { ProdutoId = produtoIds[i], Quantidade = quantidades[i] });
                }

                _context.Add(pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Nome", pedido.ClienteId);
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "Id", "Nome");
            return View(pedido);
        }

        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pedido == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido
                .Include(p => p.PedidoProdutos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Nome", pedido.ClienteId);
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "Id", "Nome");
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClienteId,DataPedido,Status,PedidoProdutos")] Pedido pedido, int[] produtoIds, int[] quantidades)
        {
            if (id != pedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPedido = await _context.Pedido
                        .Include(p => p.PedidoProdutos)
                        .FirstOrDefaultAsync(p => p.Id == id);

                    if (existingPedido == null)
                    {
                        return NotFound();
                    }

                    existingPedido.ClienteId = pedido.ClienteId;
                    existingPedido.DataPedido = pedido.DataPedido;
                    existingPedido.Status = pedido.Status;

                    existingPedido.PedidoProdutos.Clear();
                    for (int i = 0; i < produtoIds.Length; i++)
                    {
                        existingPedido.PedidoProdutos.Add(new PedidoProduto { ProdutoId = produtoIds[i], Quantidade = quantidades[i] });
                    }

                    _context.Update(existingPedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Nome", pedido.ClienteId);
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "Id", "Nome");
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pedido == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido
                .Include(p => p.Cliente)
                .Include(p => p.PedidoProdutos).ThenInclude(pp => pp.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pedido == null)
            {
                return Problem("Entity set 'techwaveContext.Pedido'  is null.");
            }
            var pedido = await _context.Pedido.FindAsync(id);
            if (pedido != null)
            {
                _context.Pedido.Remove(pedido);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return (_context.Pedido?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

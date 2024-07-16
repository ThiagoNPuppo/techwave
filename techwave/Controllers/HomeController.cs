using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using techwave.Data;


namespace techwave.Controllers
{
    public class HomeController : Controller
    {
        private readonly techwaveContext _context;

        public HomeController(techwaveContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var produtos = await _context.Produto.ToListAsync();
            return View(produtos);

            //return View(await _context.Produto.ToListAsync());
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new techwave.Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
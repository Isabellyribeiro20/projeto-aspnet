using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Patudinhos.Areas.Identity.Data;
using Patudinhos.Models;

namespace Patudinhos.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? categoria)
    {
        var query = _context.Produtos.AsQueryable();

        if (!string.IsNullOrEmpty(categoria))
            query = query.Where(p => p.Categoria == categoria);

        ViewBag.CategoriaFiltro = categoria;
        return View(await query.ToListAsync());
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View();
}

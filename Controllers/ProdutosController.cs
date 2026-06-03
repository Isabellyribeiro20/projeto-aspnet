using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Patudinhos.Areas.Identity.Data;
using Patudinhos.Models;
using System.Globalization;

namespace Patudinhos.Controllers;

public class ProdutosController : Controller
{
    private readonly AppDbContext _context;

    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Produtos — aberto para todos
    public async Task<IActionResult> Index()
    {
        return View(await _context.Produtos.ToListAsync());
    }

    // GET: Produtos/Details/5 — aberto para todos
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var produto = await _context.Produtos.FirstOrDefaultAsync(m => m.Id == id);
        if (produto == null) return NotFound();

        return View(produto);
    }

    // GET: Produtos/Create — só Admin
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Produtos/Create — só Admin
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [Bind("Id,Nome,Descricao,Preco,Estoque,Categoria")] Produto produto,
        IFormFile? foto)
    {
            if (Request.Form.TryGetValue("Preco", out var precoStr))
        {
            var str = precoStr.ToString().Replace(",", ".");
            if (decimal.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var precoCorrigido))
                produto.Preco = precoCorrigido;
        }

        if (ModelState.IsValid)
        {
            if (foto != null && foto.Length > 0)
            {
                var nomeArquivo = Guid.NewGuid() + Path.GetExtension(foto.FileName);
                var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "produtos");
                Directory.CreateDirectory(pasta);
                var caminho = Path.Combine(pasta, nomeArquivo);
                using var stream = System.IO.File.Create(caminho);
                await foto.CopyToAsync(stream);
                produto.FotoUrl = "/img/produtos/" + nomeArquivo;
            }

            _context.Add(produto);
            await _context.SaveChangesAsync();
            TempData["Sucesso"] = "Produto cadastrado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        return View(produto);
    }

    // GET: Produtos/Edit/5 — só Admin
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null) return NotFound();

        return View(produto);
    }

    // POST: Produtos/Edit/5 — só Admin
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,Nome,Descricao,Preco,Estoque,Categoria,FotoUrl")] Produto produto,
        IFormFile? foto)
    {
            if (Request.Form.TryGetValue("Preco", out var precoStr))
        {
            var str = precoStr.ToString().Replace(",", ".");
            if (decimal.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var precoCorrigido))
                produto.Preco = precoCorrigido;
        }

        if (id != produto.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (foto != null && foto.Length > 0)
                {
                    var nomeArquivo = Guid.NewGuid() + Path.GetExtension(foto.FileName);
                    var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "produtos");
                    Directory.CreateDirectory(pasta);
                    var caminho = Path.Combine(pasta, nomeArquivo);
                    using var stream = System.IO.File.Create(caminho);
                    await foto.CopyToAsync(stream);
                    produto.FotoUrl = "/img/produtos/" + nomeArquivo;
                }

                _context.Update(produto);
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Produto atualizado com sucesso!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(produto.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(produto);
    }

    // GET: Produtos/Delete/5 — só Admin
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var produto = await _context.Produtos.FirstOrDefaultAsync(m => m.Id == id);
        if (produto == null) return NotFound();

        return View(produto);
    }

    // POST: Produtos/Delete/5 — só Admin
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto != null)
            _context.Produtos.Remove(produto);

        await _context.SaveChangesAsync();
        TempData["Sucesso"] = "Produto excluído com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    private bool ProdutoExists(int id)
        => _context.Produtos.Any(e => e.Id == id);
}

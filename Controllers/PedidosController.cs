using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Patudinhos.Areas.Identity.Data;
using Patudinhos.Models;

namespace Patudinhos.Controllers;

public class PedidosController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public PedidosController(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Pedidos — só Admin vê todos os pedidos
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Pedidos.ToListAsync());
    }

    // GET: Pedidos/MeusPedidos — usuário comum vê só os seus
    [Authorize]
    public async Task<IActionResult> MeusPedidos()
    {
        var usuarioId = _userManager.GetUserId(User);
        var pedidos = await _context.Pedidos
            .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
            .Where(p => p.UsuarioId == usuarioId)
            .OrderByDescending(p => p.DataPedido)
            .ToListAsync();

        return View(pedidos);
    }

    // GET: Pedidos/Details/5
    [Authorize]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var usuarioId = _userManager.GetUserId(User);
        var pedido = await _context.Pedidos
            .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (pedido == null) return NotFound();

        // Usuário comum só pode ver os próprios pedidos
        if (!User.IsInRole("Admin") && pedido.UsuarioId != usuarioId)
            return Forbid();

        return View(pedido);
    }

    // GET: Pedidos/Create — só Admin
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Pedidos/Create — só Admin
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [Bind("Id,DataPedido,Total,Status,UsuarioId")] Pedido pedido)
    {
        if (ModelState.IsValid)
        {
            _context.Add(pedido);
            await _context.SaveChangesAsync();
            TempData["Sucesso"] = "Pedido criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        return View(pedido);
    }

    // GET: Pedidos/Edit/5 — só Admin
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido == null) return NotFound();

        return View(pedido);
    }

    // POST: Pedidos/Edit/5 — só Admin
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,DataPedido,Total,Status,UsuarioId")] Pedido pedido)
    {
        if (id != pedido.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(pedido);
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Pedido atualizado com sucesso!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(pedido.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(pedido);
    }

    // GET: Pedidos/Delete/5 — só Admin
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var pedido = await _context.Pedidos.FirstOrDefaultAsync(m => m.Id == id);
        if (pedido == null) return NotFound();

        return View(pedido);
    }

    // POST: Pedidos/Delete/5 — só Admin
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido != null)
            _context.Pedidos.Remove(pedido);

        await _context.SaveChangesAsync();
        TempData["Sucesso"] = "Pedido excluído com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    private bool PedidoExists(int id)
        => _context.Pedidos.Any(e => e.Id == id);
}

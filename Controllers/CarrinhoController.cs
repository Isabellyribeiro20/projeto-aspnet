using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Patudinhos.Areas.Identity.Data;
using Patudinhos.Models;

namespace Patudinhos.Controllers;

[Authorize]
public class CarrinhoController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private const string ChaveCarrinho = "carrinho";

    public CarrinhoController(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Lê o carrinho da sessão
    private List<ItemCarrinho> ObterCarrinho()
    {
        var json = HttpContext.Session.GetString(ChaveCarrinho);
        return string.IsNullOrEmpty(json)
            ? new List<ItemCarrinho>()
            : JsonSerializer.Deserialize<List<ItemCarrinho>>(json) ?? new();
    }

    // Salva o carrinho na sessão
    private void SalvarCarrinho(List<ItemCarrinho> itens)
        => HttpContext.Session.SetString(ChaveCarrinho, JsonSerializer.Serialize(itens));

    // GET: Carrinho
    public IActionResult Index()
    {
        var itens = ObterCarrinho();
        ViewBag.Total = itens.Sum(i => i.Subtotal);
        return View(itens);
    }

    // POST: Carrinho/Adicionar
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Adicionar(int produtoId, int quantidade = 1)
    {
        var produto = await _context.Produtos.FindAsync(produtoId);
        if (produto == null) return NotFound();

        var itens = ObterCarrinho();
        var existente = itens.FirstOrDefault(i => i.ProdutoId == produtoId);

        if (existente != null)
            existente.Quantidade += quantidade;
        else
            itens.Add(new ItemCarrinho
            {
                ProdutoId  = produto.Id,
                Nome       = produto.Nome,
                FotoUrl    = produto.FotoUrl,
                Preco      = produto.Preco,
                Quantidade = quantidade
            });

        SalvarCarrinho(itens);
        TempData["Sucesso"] = $"{produto.Nome} adicionado ao carrinho!";
        return RedirectToAction("Index", "Home");
    }

    // POST: Carrinho/Remover
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remover(int produtoId)
    {
        var itens = ObterCarrinho();
        itens.RemoveAll(i => i.ProdutoId == produtoId);
        SalvarCarrinho(itens);
        return RedirectToAction(nameof(Index));
    }

    // POST: Carrinho/AlterarQuantidade
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AlterarQuantidade(int produtoId, int quantidade)
    {
        var itens = ObterCarrinho();
        var item = itens.FirstOrDefault(i => i.ProdutoId == produtoId);

        if (item != null)
        {
            if (quantidade <= 0)
                itens.Remove(item);
            else
                item.Quantidade = quantidade;
        }

        SalvarCarrinho(itens);
        return RedirectToAction(nameof(Index));
    }

    // POST: Carrinho/Finalizar — cria o Pedido no banco
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Finalizar()
    {
        var itens = ObterCarrinho();
        if (!itens.Any())
        {
            TempData["Erro"] = "Seu carrinho está vazio.";
            return RedirectToAction(nameof(Index));
        }

        var usuarioId = _userManager.GetUserId(User);

        var pedido = new Pedido
        {
            DataPedido = DateTime.Now,
            UsuarioId  = usuarioId,
            Status     = StatusPedido.Processando,
            Total      = itens.Sum(i => i.Subtotal),
            Itens      = itens.Select(i => new ItemPedido
            {
                ProdutoId     = i.ProdutoId,
                Quantidade    = i.Quantidade,
                PrecoUnitario = i.Preco
            }).ToList()
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        // Limpa o carrinho
        HttpContext.Session.Remove(ChaveCarrinho);

        TempData["Sucesso"] = "Pedido realizado com sucesso!";
        return RedirectToAction("MeusPedidos", "Pedidos");
    }
}

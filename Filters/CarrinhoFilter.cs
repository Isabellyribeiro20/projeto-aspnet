using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Patudinhos.Models;

namespace Patudinhos.Filters;

public class CarrinhoFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Controller is Controller controller)
        {
            var session = context.HttpContext.Session;
            var json = session.GetString("carrinho");
            var itens = string.IsNullOrEmpty(json)
                ? new List<ItemCarrinho>()
                : JsonSerializer.Deserialize<List<ItemCarrinho>>(json) ?? new();

            controller.ViewBag.QtdCarrinho = itens.Sum(i => i.Quantidade);
        }
    }
}

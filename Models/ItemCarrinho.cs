namespace Patudinhos.Models;

public class ItemCarrinho
{
    public int ProdutoId   { get; set; }
    public string Nome     { get; set; } = "";
    public string? FotoUrl { get; set; }
    public decimal Preco   { get; set; }
    public int Quantidade  { get; set; }
    public decimal Subtotal => Preco * Quantidade;
}

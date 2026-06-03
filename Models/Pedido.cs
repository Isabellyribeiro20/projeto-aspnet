using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patudinhos.Models;

public class Pedido
{
    public int Id { get; set; }

    [Display(Name = "Data do Pedido")]
    public DateTime DataPedido { get; set; } = DateTime.Now;

    [Display(Name = "Total")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Total { get; set; }

    [Display(Name = "Status")]
    public StatusPedido Status { get; set; } = StatusPedido.Processando;

    // FK para o usuário dono do pedido
    public string UsuarioId { get; set; }

    // Navegação — um Pedido tem muitos ItensPedido (1:N)
    public ICollection<ItemPedido>? Itens { get; set; }
}

public enum StatusPedido
{
    Processando,
    [Display(Name = "Em trânsito")]
    EmTransito,
    Entregue,
    Cancelado
}

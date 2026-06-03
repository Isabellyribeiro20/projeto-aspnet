using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patudinhos.Models;

public class ItemPedido
{
    public int Id { get; set; }

    // FK para Pedido
    [Required]
    public int PedidoId { get; set; }

    // FK para Produto
    [Required]
    public int ProdutoId { get; set; }

    [Required(ErrorMessage = "A quantidade é obrigatória")]
    [Display(Name = "Quantidade")]
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser pelo menos 1")]
    public int Quantidade { get; set; }

    [Display(Name = "Preço Unitário")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecoUnitario { get; set; }

    // Propriedade calculada — não vira coluna no banco
    [NotMapped]
    [Display(Name = "Subtotal")]
    public decimal Subtotal => PrecoUnitario * Quantidade;

    // Navegação
    public Pedido? Pedido { get; set; }
    public Produto? Produto { get; set; }
}

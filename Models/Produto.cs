using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patudinhos.Models;

public class Produto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [Display(Name = "Nome")]
    public string Nome { get; set; } = "";

    [Required(ErrorMessage = "A descrição é obrigatória")]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = "";

    [Required(ErrorMessage = "O preço é obrigatório")]
    [Display(Name = "Preço")]
    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "O estoque é obrigatório")]
    [Display(Name = "Estoque")]
    [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo")]
    public int Estoque { get; set; }

    [Required(ErrorMessage = "A categoria é obrigatória")]
    [Display(Name = "Categoria")]
    public string Categoria { get; set; } = "";

    [Display(Name = "Foto")]
    public string? FotoUrl { get; set; }

    // Navegação
    public ICollection<ItemPedido>? ItensPedido { get; set; }
}

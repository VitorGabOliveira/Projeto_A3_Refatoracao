using System.ComponentModel.DataAnnotations;

namespace ApiPrimeiroSimulado.Dtos.Produto
{
    public class ProdutoRequestDto
    {
        [Required]
        public string nomeProduto { get; set; }

        [Required]
        public string categoria { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero.")]
        public decimal preco { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa.")]
        public int quantidadeProduto { get; set; }
    }
}
    
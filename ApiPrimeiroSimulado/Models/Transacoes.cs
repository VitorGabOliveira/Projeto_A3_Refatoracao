using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiPrimeiroSimulado.Models
{
    public class Transacoes
    {
        [Key]
        public int? idTransacao { get; set; }
        public int produtoId { get; set; }
        public int quantidadeTransacao { get; set; }
        public DateTime dataTransacao { get; set; } = DateTime.Now;
        [Required]
        public required string tipoTransacao { get; set; }

        [JsonIgnore]
        public Produtos? produtos { get; set; }
    }
}

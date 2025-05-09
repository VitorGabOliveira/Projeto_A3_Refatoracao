using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiPrimeiroSimulado.Models
{
    public class Produtos
    {
        [Key]
        public int idProduto { get; set; }
        public string nomeProduto { get; set; }

        public string categoria { get; set; }

        public decimal preco { get; set; }

        public int quantidadeProduto { get; set; }

       public DateTime dataCadastro { get; set; }

        [JsonIgnore]
        public ICollection<Transacoes>? Transacoes { get; set; }
    }
}

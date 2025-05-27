namespace ApiPrimeiroSimulado.Dtos.Produto
{
    public class ProdutoResponseDto
    {
        public int idProduto { get; set; }
        public string nomeProduto { get; set; }
        public string categoria { get; set; }
        public decimal preco { get; set; }
        public int quantidadeProduto { get; set; }
        public DateTime dataCadastro { get; set; }
    }
}

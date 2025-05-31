namespace ApiPrimeiroSimulado.Dtos.Transacao;

public class TransacaoResponseDto
{
    public int idTransacao { get; set; }
    public int produtoId { get; set; }
    public string tipoTransacao { get; set; } = string.Empty;
    public int quantidadeTransacao { get; set; }
    public DateTime dataTransacao { get; set; }
}

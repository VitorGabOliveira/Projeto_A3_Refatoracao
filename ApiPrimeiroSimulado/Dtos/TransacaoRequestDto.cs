namespace ApiPrimeiroSimulado.Dtos.Transacao;

public class TransacaoRequestDto
{
    public int produtoId { get; set; }
    public string tipoTransacao { get; set; } = string.Empty;
    public int quantidadeTransacao { get; set; }
}

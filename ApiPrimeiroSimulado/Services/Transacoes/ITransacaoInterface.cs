using ApiPrimeiroSimulado.Dtos.Transacao;
using ApiPrimeiroSimulado.Models;

namespace ApiPrimeiroSimulado.Services.Transacoes;

public interface ITransacaoInterface
{
    Task<ResponseModel<IEnumerable<TransacaoResponseDto>>> GetAllAsync();
    Task<ResponseModel<TransacaoResponseDto>> GetByIdAsync(int id);
    Task<ResponseModel<TransacaoResponseDto>> CreateAsync(TransacaoRequestDto dto);
    Task<ResponseModel<string>> UpdateAsync(int id, TransacaoRequestDto dto);
    Task<ResponseModel<string>> DeleteAsync(int id);
}

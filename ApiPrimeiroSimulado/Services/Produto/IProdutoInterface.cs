using ApiPrimeiroSimulado.Dtos;
using ApiPrimeiroSimulado.Dtos.Produto;
using ApiPrimeiroSimulado.Models;

namespace ApiPrimeiroSimulado.Services.Produto
{
    public interface IProdutoInterface
    {
        Task<ResponseModel<IEnumerable<ProdutoResponseDto>>> GetAllAsync();
        Task<ResponseModel<ProdutoResponseDto>> GetByIdAsync(int id);
        Task<ResponseModel<ProdutoResponseDto>> CreateAsync(ProdutoRequestDto novoProduto);
        Task<ResponseModel<string>> UpdateAsync(int id, ProdutoRequestDto produtoAtualizado);
        Task<ResponseModel<string>> DeleteAsync(int id);
    }
}

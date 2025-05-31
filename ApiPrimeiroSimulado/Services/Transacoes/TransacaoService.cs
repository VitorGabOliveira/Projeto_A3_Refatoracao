using ApiPrimeiroSimulado.Data;
using ApiPrimeiroSimulado.Dtos.Transacao;
using ApiPrimeiroSimulado.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPrimeiroSimulado.Services.Transacoes;

public class TransacaoService : ITransacaoInterface
{
    private readonly AppDbContext _context;

    public TransacaoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseModel<IEnumerable<TransacaoResponseDto>>> GetAllAsync()
    {
        var transacoes = await _context.Transacoes
            .Select(t => new TransacaoResponseDto
            {
                idTransacao = t.idTransacao,
                produtoId = t.produtoId,
                tipoTransacao = t.tipoTransacao,
                quantidadeTransacao = t.quantidadeTransacao,
                dataTransacao = t.dataTransacao
            })
            .ToListAsync();

        return new ResponseModel<IEnumerable<TransacaoResponseDto>>(true, "Transações encontradas", transacoes);
    }

    public async Task<ResponseModel<TransacaoResponseDto>> GetByIdAsync(int id)
    {
        var t = await _context.Transacoes.FindAsync(id);
        if (t == null)
            return new ResponseModel<TransacaoResponseDto>(false, "Transação não encontrada");

        return new ResponseModel<TransacaoResponseDto>(true, "Transação encontrada", new TransacaoResponseDto
        {
            idTransacao = t.idTransacao,
            produtoId = t.produtoId,
            tipoTransacao = t.tipoTransacao,
            quantidadeTransacao = t.quantidadeTransacao,
            dataTransacao = t.dataTransacao
        });
    }

    public async Task<ResponseModel<TransacaoResponseDto>> CreateAsync(TransacaoRequestDto dto)
    {
        var produto = await _context.Produtos.FindAsync(dto.produtoId);
        if (produto == null)
            return new ResponseModel<TransacaoResponseDto>(false, $"Produto com ID {dto.produtoId} não encontrado.");

        switch (dto.tipoTransacao.ToLowerInvariant())
        {
            case "compra":
                produto.quantidadeProduto += dto.quantidadeTransacao;
                break;
            case "venda":
                if (produto.quantidadeProduto < dto.quantidadeTransacao)
                    return new ResponseModel<TransacaoResponseDto>(false, "Estoque insuficiente.");
                produto.quantidadeProduto -= dto.quantidadeTransacao;
                break;
            case "ajuste":
                produto.quantidadeProduto = dto.quantidadeTransacao;
                break;
            default:
                return new ResponseModel<TransacaoResponseDto>(false, "Tipo de transação inválido.");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Produtos.Update(produto);

            var novaTransacao = new TransacaoModel
            {
                produtoId = dto.produtoId,
                tipoTransacao = dto.tipoTransacao,
                quantidadeTransacao = dto.quantidadeTransacao,
                dataTransacao = DateTime.Now
            };

            _context.Transacoes.Add(novaTransacao);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var responseDto = new TransacaoResponseDto
            {
                idTransacao = novaTransacao.idTransacao,
                produtoId = novaTransacao.produtoId,
                tipoTransacao = novaTransacao.tipoTransacao,
                quantidadeTransacao = novaTransacao.quantidadeTransacao,
                dataTransacao = novaTransacao.dataTransacao
            };

            return new ResponseModel<TransacaoResponseDto>(true, "Transação criada", responseDto);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<ResponseModel<string>> UpdateAsync(int id, TransacaoRequestDto dto)
    {
        var transacao = await _context.Transacoes.FindAsync(id);
        if (transacao == null)
            return new ResponseModel<string>(false, "Transação não encontrada");

        transacao.tipoTransacao = dto.tipoTransacao;
        transacao.quantidadeTransacao = dto.quantidadeTransacao;
        transacao.produtoId = dto.produtoId;

        _context.Entry(transacao).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return new ResponseModel<string>(true, "Transação atualizada");
    }

    public async Task<ResponseModel<string>> DeleteAsync(int id)
    {
        var transacao = await _context.Transacoes.FindAsync(id);
        if (transacao == null)
            return new ResponseModel<string>(false, "Transação não encontrada");

        _context.Transacoes.Remove(transacao);
        await _context.SaveChangesAsync();

        return new ResponseModel<string>(true, "Transação excluída");
    }
}

using ApiPrimeiroSimulado.Data;
using ApiPrimeiroSimulado.Dtos.Transacao;
using ApiPrimeiroSimulado.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPrimeiroSimulado.Services.Transacoes;

public class TransacaoService : ITransacaoInterface
{
    private readonly AppDbContext _context;

    // Construtor que injeta o contexto do banco de dados
    public TransacaoService(AppDbContext context)
    {
        _context = context;
    }

    // Retorna todas as transações existentes no banco, convertidas para DTOs
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

    // Retorna uma transação pelo ID, ou uma resposta com status false se não encontrada
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

    // Cria uma nova transação e atualiza o estoque do produto de acordo com o tipo de transação
    public async Task<ResponseModel<TransacaoResponseDto>> CreateAsync(TransacaoRequestDto dto)
    {
        // Verifica se o produto informado existe
        var produto = await _context.Produtos.FindAsync(dto.produtoId);
        if (produto == null)
            return new ResponseModel<TransacaoResponseDto>(false, $"Produto com ID {dto.produtoId} não encontrado.");

        // Aplica a lógica de estoque com base no tipo de transação
        switch (dto.tipoTransacao.ToLowerInvariant())
        {
            case "compra":
                produto.quantidadeProduto += dto.quantidadeTransacao; // Adiciona ao estoque
                break;
            case "venda":
                if (produto.quantidadeProduto < dto.quantidadeTransacao)
                    return new ResponseModel<TransacaoResponseDto>(false, "Estoque insuficiente.");
                produto.quantidadeProduto -= dto.quantidadeTransacao; // Subtrai do estoque
                break;
            case "ajuste":
                produto.quantidadeProduto = dto.quantidadeTransacao; // Ajusta diretamente a quantidade
                break;
            default:
                return new ResponseModel<TransacaoResponseDto>(false, "Tipo de transação inválido.");
        }

        // Inicia uma transação de banco de dados para garantir integridade
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Atualiza a quantidade do produto no banco
            _context.Produtos.Update(produto);

            // Cria o objeto da nova transação
            var novaTransacao = new TransacaoModel
            {
                produtoId = dto.produtoId,
                tipoTransacao = dto.tipoTransacao,
                quantidadeTransacao = dto.quantidadeTransacao,
                dataTransacao = DateTime.Now
            };

            // Adiciona e salva a transação
            _context.Transacoes.Add(novaTransacao);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync(); // Confirma a transação do banco

            // Cria o DTO de resposta com os dados da transação criada
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
            await transaction.RollbackAsync(); // Desfaz alterações em caso de erro
            throw;
        }
    }

    // Atualiza os dados de uma transação existente
    public async Task<ResponseModel<string>> UpdateAsync(int id, TransacaoRequestDto dto)
    {
        var transacao = await _context.Transacoes.FindAsync(id);
        if (transacao == null)
            return new ResponseModel<string>(false, "Transação não encontrada");

        // Atualiza os campos com base no DTO recebido
        transacao.tipoTransacao = dto.tipoTransacao;
        transacao.quantidadeTransacao = dto.quantidadeTransacao;
        transacao.produtoId = dto.produtoId;

        _context.Entry(transacao).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return new ResponseModel<string>(true, "Transação atualizada");
    }

    // Remove uma transação do banco com base no ID
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

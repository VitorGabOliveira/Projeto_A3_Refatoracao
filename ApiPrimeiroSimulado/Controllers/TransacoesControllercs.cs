using ApiPrimeiroSimulado.Data;
using ApiPrimeiroSimulado.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPrimeiroSimulado.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransacoesController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransacoesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]

    public async Task<ActionResult<IEnumerable<Transacoes>>> GetTransacoes()
    {
        return await _context.Transacoes.ToListAsync();
    }

    [HttpGet("{id}")]

    public async Task<ActionResult<Transacoes>> GetTransacoes(int id)
    {
        var transacao = await _context.Transacoes.FindAsync(id);

        if (transacao == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(transacao);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Transacoes>> CreateTransacoes(Transacoes transacao)
    {
        var produto = await _context.Produtos.FindAsync(transacao.produtoId);
        if (produto == null)
            return NotFound($"Produto com id {transacao.produtoId} não encontrado.");

        switch (transacao.tipoTransacao?.ToLowerInvariant())
        {
            case "compra":
                produto.quantidadeProduto += transacao.quantidadeTransacao; 
                break;

            case "venda":
                if (produto.quantidadeProduto < transacao.quantidadeTransacao)
                    return BadRequest("Estoque insuficiente para realizar a venda.");

                produto.quantidadeProduto -= transacao.quantidadeTransacao;
                break;

            case "ajuste":
                produto.quantidadeProduto = transacao.quantidadeTransacao;
                break;

            default:
                return BadRequest("Tipo de transação inválido. Use 'Compra', 'Venda' ou 'Ajuste'.");
        }

        
        using var dbTrans = await _context.Database.BeginTransactionAsync();
        try
        {
            // Atualiza o estoque do produto
            _context.Produtos.Update(produto);

            // Registra a transação
            transacao.dataTransacao = DateTime.Now;
            _context.Transacoes.Add(transacao);

            // Persiste tudo
            await _context.SaveChangesAsync();
            await dbTrans.CommitAsync();
        }
        catch
        {
            await dbTrans.RollbackAsync();
            throw;
        }

        // 4. Retorna Created com os dados da transação
        return CreatedAtAction(
            nameof(GetTransacoes),
            new { id = transacao.idTransacao },
            transacao
        );
    }


    [HttpPut("{id}")]

    public async Task<IActionResult> UpdateTransacoes(int id, Transacoes transacao)
    {
        if (id != transacao.idTransacao)
        {
            return BadRequest("O id informado não foi encontrado");
        }
        _context.Entry(transacao).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            if (!TransacaoExist(id))
            {
                return NotFound(ex);
            }
            else
            {
                throw;
            }
        }
        return NoContent();
    }

    [HttpDelete("{id}")]

    public async Task<IActionResult> DeleteTransacoes(int id)
    {
        var transacao = await _context.Transacoes.FindAsync(id);
        if (transacao == null)
        {
            return NotFound();
        }
        try
        {
            _context.Transacoes.Remove(transacao);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception)
        {
            throw;
        }
    }

    private bool TransacaoExist(int id)
    {
        return _context.Transacoes.Any(p => p.idTransacao == id);
    }
}


using Microsoft.AspNetCore.Mvc;
using ApiPrimeiroSimulado.Models;

namespace ApiPrimeiroSimulado.Services.Transacoes;

public class TransacaoService: ITransacaoInterface
{
    public String GetTransacoes()
    {
        return "";
    }

    public String GetTransacoesById(int id)
    {
        return "";
    }

    public Task<ActionResult> PostTransacoes(TransacaoModel novoTransacao)
    {
        return;
    }

    public Task<ActionResult> PutTransacoes(TransacaoModel transacao, int idEdit)
    {
        return;
    }

    public Task<ActionResult> DeleteTransacoes(int id)
    {
        return;
    }
}

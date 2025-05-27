using ApiPrimeiroSimulado.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiPrimeiroSimulado.Services.UsuarioController;

public class UsuarioService: IUsuarioInterface
{
    public String GetUsuarios()
    {
        return "";
    }

    public String GetUsuarioById(int id)
    {
        return "";
    }

    public Task<ActionResult> PostTransacoes(UsuarioModel novoUsuario)
    {
        return;
    }

    public Task<ActionResult> PutTransacoes(UsuarioModel novoUsuario, int idEdit)
    {
        return;
    }

    public Task<ActionResult> DeleteUsuario(int id)
    {
        return;
    }
}
}

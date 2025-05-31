using ApiPrimeiroSimulado.Data;
using ApiPrimeiroSimulado.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPrimeiroSimulado.Services.Usuario;

public class UsuarioService : IUsuarioInterface
{
    private readonly AppDbContext _context;

    public UsuarioService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseModel<IEnumerable<UsuarioModel>>> GetUsuarios()
    {
        var usuarios = await _context.Usuarios.ToListAsync();
        return new ResponseModel<IEnumerable<UsuarioModel>>
        {
            status = true,
            mensagem = "Usuários encontrados",
            dados = usuarios
        };
    }

    public async Task<ResponseModel<UsuarioModel>> GetUsuarioById(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return new ResponseModel<UsuarioModel>
            {
                status = false,
                mensagem = "Usuário não encontrado",
                dados = null
            };
        }

        return new ResponseModel<UsuarioModel>
        {
            status = true,
            mensagem = "Usuário encontrado",
            dados = usuario
        };
    }

    public async Task<ResponseModel<UsuarioModel>> PostUsuario(UsuarioModel novoUsuario)
    {
        _context.Usuarios.Add(novoUsuario);
        await _context.SaveChangesAsync();

        return new ResponseModel<UsuarioModel>
        {
            status = true,
            mensagem = "Usuário criado com sucesso",
            dados = novoUsuario
        };
    }

    public async Task<ResponseModel<string>> PutUsuario(int id, UsuarioModel usuarioAtualizado)
    {
        if (id != usuarioAtualizado.idUsuario)
        {
            return new ResponseModel<string>
            {
                status = false,
                mensagem = "Id informado não corresponde ao usuário",
                dados = null
            };
        }

        _context.Entry(usuarioAtualizado).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return new ResponseModel<string>
            {
                status = true,
                mensagem = "Usuário atualizado com sucesso"
            };
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Usuarios.Any(u => u.idUsuario == id))
            {
                return new ResponseModel<string>
                {
                    status = false,
                    mensagem = "Usuário não encontrado"
                };
            }
            throw;
        }
    }

    public async Task<ResponseModel<string>> DeleteUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return new ResponseModel<string>
            {
                status = false,
                mensagem = "Usuário não encontrado"
            };
        }

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return new ResponseModel<string>
        {
            status = true,
            mensagem = "Usuário excluído com sucesso"
        };
    }
}

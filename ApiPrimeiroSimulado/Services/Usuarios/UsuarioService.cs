using ApiPrimeiroSimulado.Data;
using ApiPrimeiroSimulado.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPrimeiroSimulado.Services.Usuario;

public class UsuarioService : IUsuarioInterface
{
    private readonly AppDbContext _context;

    // O construtor recebe uma instância do AppDbContext (injeção de dependência), 
    // que é usada para acessar o banco de dados por meio do Entity Framework Core.
    public UsuarioService(AppDbContext context)
    {
        _context = context;
    }

    // O método GetUsuarios busca todos os usuários cadastrados no banco.
    // Ele retorna um objeto do tipo ResponseModel com uma lista de usuários,
    // além de uma mensagem e um status indicando sucesso.
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

    // O método GetUsuarioById recebe um ID e tenta encontrar o usuário correspondente no banco.
    // Caso não encontre, retorna uma resposta com status false e mensagem de erro.
    // Se encontrar, retorna o usuário encontrado dentro de um ResponseModel.
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

    // O método PostUsuario insere um novo usuário no banco de dados.
    // Após adicionar o usuário ao contexto e salvar as alterações, 
    // retorna um ResponseModel indicando sucesso e contendo o novo usuário.
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

    // O método PutUsuario atualiza um usuário existente no banco de dados.
    // Ele primeiro verifica se o ID informado na URL corresponde ao ID do objeto recebido.
    // Se não corresponder, retorna erro. Se corresponder, tenta atualizar o banco.
    // Em caso de conflito de concorrência ou ID inexistente, trata a exceção adequadamente.
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

    // O método DeleteUsuario remove um usuário do banco de dados com base no ID informado.
    // Se o usuário for encontrado, ele é removido e uma resposta de sucesso é retornada.
    // Caso contrário, é retornada uma resposta com status false e mensagem de erro.
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

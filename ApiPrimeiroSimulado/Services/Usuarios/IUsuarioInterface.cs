using ApiPrimeiroSimulado.Models;

namespace ApiPrimeiroSimulado.Services.Usuario
{
    public interface IUsuarioInterface
    {
        Task<ResponseModel<IEnumerable<UsuarioModel>>> GetUsuarios();
        Task<ResponseModel<UsuarioModel>> GetUsuarioById(int id);
        Task<ResponseModel<UsuarioModel>> PostUsuario(UsuarioModel novoUsuario);
        Task<ResponseModel<string>> PutUsuario(int id, UsuarioModel usuarioAtualizado);
        Task<ResponseModel<string>> DeleteUsuario(int id);
    }
}

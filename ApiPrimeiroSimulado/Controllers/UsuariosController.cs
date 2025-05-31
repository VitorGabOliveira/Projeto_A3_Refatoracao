using ApiPrimeiroSimulado.Models;
using ApiPrimeiroSimulado.Services.Usuario;
using Microsoft.AspNetCore.Mvc;

namespace ApiPrimeiroSimulado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioInterface _usuarioService;

        public UsuariosController(IUsuarioInterface usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseModel<IEnumerable<UsuarioModel>>>> GetUsuarios()
        {
            return Ok(await _usuarioService.GetUsuarios());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel<UsuarioModel>>> GetUsuario(int id)
        {
            var response = await _usuarioService.GetUsuarioById(id);
            if (!response.status) return NotFound(response);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseModel<UsuarioModel>>> CreateUsuario(UsuarioModel usuario)
        {
            var response = await _usuarioService.PostUsuario(usuario);
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.idUsuario }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseModel<string>>> UpdateUsuario(int id, UsuarioModel usuario)
        {
            var response = await _usuarioService.PutUsuario(id, usuario);
            if (!response.status) return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseModel<string>>> DeleteUsuario(int id)
        {
            var response = await _usuarioService.DeleteUsuario(id);
            if (!response.status) return NotFound(response);
            return Ok(response);
        }
    }
}

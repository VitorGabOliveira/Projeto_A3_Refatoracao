using ApiPrimeiroSimulado.Data;
using ApiPrimeiroSimulado.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPrimeiroSimulado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<UsuarioModel>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<UsuarioModel>> GetUsuarios(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(usuario);
            }
        }

        [HttpPost]

        public async Task<ActionResult<UsuarioModel>> CreateUsuarios(UsuarioModel usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsuarios), new { id = usuario.idUsuario }, usuario);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateUsuarios(int id, UsuarioModel usuario)
        {
            if (id != usuario.idUsuario)
            {
                return BadRequest("O id informado não foi encontrado");
            }
            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!UsuarioExist(id))
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

        public async Task<IActionResult> DeleteUsuarios(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            try
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool UsuarioExist(int id)
        {
            return _context.Usuarios.Any(p => p.idUsuario == id);
        }
    }
}

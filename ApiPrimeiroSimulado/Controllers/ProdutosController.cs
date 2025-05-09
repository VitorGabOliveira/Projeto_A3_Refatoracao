using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiPrimeiroSimulado.Models;


using ApiPrimeiroSimulado.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiPrimeiroSimulado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<Produtos>>> GetProdutos()
        {
            return await _context.Produtos.ToListAsync();
        }

        [HttpGet("{id}")]
        
        public async Task<ActionResult<Produtos>> GetProdutos(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if(produto == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(produto);
            }
        }

        [HttpPost]

        public async Task<ActionResult<Produtos>> CreateProdutos(Produtos produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProdutos), new { id = produto.idProduto }, produto);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateProdutos(int id, Produtos produto)
        {
            if (id != produto.idProduto)
            {
                return BadRequest("O id informado não foi encontrado");
            }
            _context.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!ProdutoExist(id))
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

        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if(produto == null)
            {
                return NotFound();
            }
            try { 
                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch(Exception)
            {
                throw;
            }
        }

        private bool ProdutoExist(int id)
        {
            return _context.Produtos.Any(p => p.idProduto == id);
        }
    }
}

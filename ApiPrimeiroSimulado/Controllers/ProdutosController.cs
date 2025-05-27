using ApiPrimeiroSimulado.Dtos.Produto; // DTOs específicos para Produto (request e response)
using ApiPrimeiroSimulado.Models; // Modelos da aplicação 
using ApiPrimeiroSimulado.Services.Produto; // Interface do serviço de Produto
using Microsoft.AspNetCore.Mvc; // Recursos para criação de controllers e rotas HTTP

namespace ApiPrimeiroSimulado.Controllers
{
    // Define a rota base para este controller como "api/produtos"
    [Route("api/[controller]")]
    [ApiController] // Indica que esse controller lida com requisições de API REST
    public class ProdutosController : ControllerBase
    {
        // Injeta a dependência do serviço de produto
        private readonly IProdutoInterface _produtoService;

        // Construtor que recebe o serviço via injeção de dependência
        public ProdutosController(IProdutoInterface produtoService)
        {
            _produtoService = produtoService;
        }

        // ======================
        // ROTA: GET /api/produtos
        // ======================
        // Retorna todos os produtos cadastrados
        [HttpGet]
        public async Task<ActionResult<ResponseModel<IEnumerable<ProdutoResponseDto>>>> GetAll()
        {
            var response = await _produtoService.GetAllAsync();
            return Ok(response); // Retorna status 200 com o corpo da resposta
        }

        // =============================
        // ROTA: GET /api/produtos/{id}
        // =============================
        // Retorna um produto específico pelo seu ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel<ProdutoResponseDto>>> GetById(int id)
        {
            var response = await _produtoService.GetByIdAsync(id);

            // Se o produto não for encontrado, retorna 404
            if (!response.status)
                return NotFound(response);

            // Se encontrado, retorna 200 com os dados
            return Ok(response);
        }

        // ========================
        // ROTA: POST /api/produtos
        // ========================
        // Cria um novo produto
        [HttpPost]
        public async Task<ActionResult<ResponseModel<ProdutoResponseDto>>> Create([FromBody] ProdutoRequestDto dto)
        {
            var response = await _produtoService.CreateAsync(dto);

            // Retorna status 201 (Created), com a URL de acesso ao novo recurso no cabeçalho Location
            return CreatedAtAction(
                nameof(GetById), // Nome da action para montar o link de retorno
                new { id = response.dados?.idProduto }, // Parâmetro que será usado na rota do GetById
                response // Corpo da resposta com o produto criado
            );
        }

        // ==============================
        // ROTA: PUT /api/produtos/{id}
        // ==============================
        // Atualiza os dados de um produto existente
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseModel<string>>> Update(int id, [FromBody] ProdutoRequestDto dto)
        {
            var response = await _produtoService.UpdateAsync(id, dto);

            // Se o produto não for encontrado, retorna 404
            if (!response.status)
                return NotFound(response);

            // Retorna 200 com a mensagem de sucesso
            return Ok(response);
        }

        // ================================
        // ROTA: DELETE /api/produtos/{id}
        // ================================
        // Remove um produto pelo ID
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseModel<string>>> Delete(int id)
        {
            var response = await _produtoService.DeleteAsync(id);

            // Se não for encontrado, retorna 404
            if (!response.status)
                return NotFound(response);

            // Caso contrário, retorna 200 com mensagem de sucesso
            return Ok(response);
        }
    }
}

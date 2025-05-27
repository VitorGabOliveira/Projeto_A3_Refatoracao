using ApiPrimeiroSimulado.Data; // Acesso ao DbContext da aplicação (banco de dados)
using ApiPrimeiroSimulado.Dtos.Produto; // DTOs de Produto (para entrada e saída de dados)
using ApiPrimeiroSimulado.Models; // Modelo de domínio ProdutoModel
using Microsoft.EntityFrameworkCore; // Funcionalidades do Entity Framework Core (como ToListAsync, FindAsync etc.)

namespace ApiPrimeiroSimulado.Services.Produto
{
    // Classe responsável por implementar a interface IProdutoService
    public class ProdutoService : IProdutoInterface
    {
        // Campo privado para acessar o contexto do banco de dados
        private readonly AppDbContext _context;

        // Construtor que recebe o AppDbContext via injeção de dependência
        public ProdutoService(AppDbContext context)
        {
            _context = context;
        }

        // Método que retorna todos os produtos cadastrados
        public async Task<ResponseModel<IEnumerable<ProdutoResponseDto>>> GetAllAsync()
        {
            // Busca todos os produtos do banco de dados de forma assíncrona
            var produtos = await _context.Produtos.ToListAsync();

            // Converte cada produto encontrado para um DTO de resposta
            var produtosDto = produtos.Select(p => new ProdutoResponseDto
            {
                idProduto = p.idProduto,
                nomeProduto = p.nomeProduto,
                categoria = p.categoria,
                preco = p.preco,
                quantidadeProduto = p.quantidadeProduto,
                dataCadastro = p.dataCadastro
            });

            // Retorna a resposta com a lista de produtos convertida para DTO
            return new ResponseModel<IEnumerable<ProdutoResponseDto>>
            {
                dados = produtosDto,
                mensagem = "Produtos encontrados com sucesso.",
                status = true
            };
        }

        // Método que busca um único produto pelo seu ID
        public async Task<ResponseModel<ProdutoResponseDto>> GetByIdAsync(int id)
        {
            // Busca o produto pelo ID
            var produto = await _context.Produtos.FindAsync(id);

            // Caso o produto não exista, retorna uma resposta com status falso
            if (produto == null)
            {
                return new ResponseModel<ProdutoResponseDto>
                {
                    status = false,
                    mensagem = "Produto não encontrado.",
                    dados = null
                };
            }

            // Converte o produto encontrado para um DTO de resposta
            var dto = new ProdutoResponseDto
            {
                idProduto = produto.idProduto,
                nomeProduto = produto.nomeProduto,
                categoria = produto.categoria,
                preco = produto.preco,
                quantidadeProduto = produto.quantidadeProduto,
                dataCadastro = produto.dataCadastro
            };

            // Retorna o produto encontrado com sucesso
            return new ResponseModel<ProdutoResponseDto>
            {
                status = true,
                mensagem = "Produto encontrado com sucesso.",
                dados = dto
            };
        }

        // Método que cria um novo produto no banco de dados
        public async Task<ResponseModel<ProdutoResponseDto>> CreateAsync(ProdutoRequestDto novoProduto)
        {
            // Cria uma nova instância de ProdutoModel com os dados vindos do DTO
            var produto = new ProdutoModel
            {
                nomeProduto = novoProduto.nomeProduto,
                categoria = novoProduto.categoria,
                preco = novoProduto.preco,
                quantidadeProduto = novoProduto.quantidadeProduto
            };

            // Adiciona o produto no contexto para inserção no banco
            _context.Produtos.Add(produto);

            // Salva as alterações de forma assíncrona
            await _context.SaveChangesAsync();

            // Converte o produto recém-criado em um DTO de resposta
            var dto = new ProdutoResponseDto
            {
                idProduto = produto.idProduto,
                nomeProduto = produto.nomeProduto,
                categoria = produto.categoria,
                preco = produto.preco,
                quantidadeProduto = produto.quantidadeProduto,
                dataCadastro = produto.dataCadastro
            };

            // Retorna o produto criado com status positivo
            return new ResponseModel<ProdutoResponseDto>
            {
                status = true,
                mensagem = "Produto criado com sucesso.",
                dados = dto
            };
        }

        // Método que atualiza um produto já existente
        public async Task<ResponseModel<string>> UpdateAsync(int id, ProdutoRequestDto produtoAtualizado)
        {
            // Busca o produto pelo ID
            var produto = await _context.Produtos.FindAsync(id);

            // Se não for encontrado, retorna mensagem de erro
            if (produto == null)
            {
                return new ResponseModel<string>
                {
                    status = false,
                    mensagem = "Produto não encontrado.",
                    dados = null
                };
            }

            // Atualiza os campos do produto com os novos dados vindos do DTO
            produto.nomeProduto = produtoAtualizado.nomeProduto;
            produto.categoria = produtoAtualizado.categoria;
            produto.preco = produtoAtualizado.preco;
            produto.quantidadeProduto = produtoAtualizado.quantidadeProduto;

            try
            {
                // Tenta salvar as alterações no banco
                await _context.SaveChangesAsync();

                // Retorna mensagem de sucesso
                return new ResponseModel<string>
                {
                    status = true,
                    mensagem = "Produto atualizado com sucesso.",
                    dados = null
                };
            }
            catch
            {
                // Em caso de erro, relança a exceção (pode ser melhorado para logar o erro)
                throw;
            }
        }

        // Método que exclui um produto existente
        public async Task<ResponseModel<string>> DeleteAsync(int id)
        {
            // Busca o produto pelo ID
            var produto = await _context.Produtos.FindAsync(id);

            // Se não for encontrado, retorna resposta com status falso
            if (produto == null)
            {
                return new ResponseModel<string>
                {
                    status = false,
                    mensagem = "Produto não encontrado.",
                    dados = null
                };
            }

            // Remove o produto do contexto
            _context.Produtos.Remove(produto);

            // Salva as alterações para efetivar a remoção
            await _context.SaveChangesAsync();

            // Retorna mensagem de sucesso
            return new ResponseModel<string>
            {
                status = true,
                mensagem = "Produto deletado com sucesso.",
                dados = null
            };
        }
    }
}

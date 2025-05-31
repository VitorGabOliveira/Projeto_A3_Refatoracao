using ApiPrimeiroSimulado.Data;
using Microsoft.EntityFrameworkCore;
using ApiPrimeiroSimulado.Services.Produto;
using ApiPrimeiroSimulado.Services.Transacoes;
using ApiPrimeiroSimulado.Services.Usuario;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Injeção de dependência do service
builder.Services.AddScoped<IProdutoInterface, ProdutoService>();
builder.Services.AddScoped<ITransacaoInterface, TransacaoService>();
builder.Services.AddScoped<IUsuarioInterface, UsuarioService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();

namespace ApiPrimeiroSimulado.Data;

using ApiPrimeiroSimulado.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext: DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    { 
       
    }
    public DbSet<ProdutoModel> Produtos { get; set; }
    public DbSet<TransacaoModel> Transacoes { get; set; }
    public DbSet<UsuarioModel> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TransacaoModel>()
            .HasOne( t => t.produtos)
            .WithMany(p => p.Transacoes)
            .HasForeignKey(t => t.produtoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

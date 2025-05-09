namespace ApiPrimeiroSimulado.Data;

using ApiPrimeiroSimulado.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext: DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    { 
       
    }
    public DbSet<Produtos> Produtos { get; set; }
    public DbSet<Transacoes> Transacoes { get; set; }
    public DbSet<Usuarios> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Transacoes>()
            .HasOne( t => t.produtos)
            .WithMany(p => p.Transacoes)
            .HasForeignKey(t => t.produtoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

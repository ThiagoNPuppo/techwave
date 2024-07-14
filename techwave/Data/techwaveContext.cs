using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using techwave.Models;


namespace techwave.Data
{
    public class techwaveContext : DbContext
    {
        public techwaveContext (DbContextOptions<techwaveContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; } = default!;

        public DbSet<Produto>? Produto { get; set; }

        public DbSet<Endereco>? Endereco { get; set; }

        public DbSet<Funcionario>? Funcionario { get; set; }

        public DbSet<Cliente>? Cliente { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<PedidoProduto> PedidoProduto { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>(entity =>
            {
                entity.Property(e => e.Preco).HasColumnType("decimal(18,2)");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

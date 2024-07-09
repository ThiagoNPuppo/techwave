using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjetoFinal.Models;

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
    }
}

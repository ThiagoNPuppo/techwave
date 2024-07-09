// Produto.cs
using System.ComponentModel.DataAnnotations;

namespace ProjetoFinal.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        [Range(0.01, 10000.00)]
        public decimal Preco { get; set; }

        public int Estoque { get; set; }
    }
}

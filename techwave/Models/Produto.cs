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
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^\d+(\,\d{1,2})?$", ErrorMessage = "Preço deve ser um valor decimal com até duas casas decimais.")]
        public decimal Preco { get; set; }


        public int Estoque { get; set; }
    }
}

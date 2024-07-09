using System.ComponentModel.DataAnnotations;

namespace ProjetoFinal.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        //[Required]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required]
        public string Telefone { get; set; }

        public int EnderecoId { get; set; }
        public Endereco Endereco { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}

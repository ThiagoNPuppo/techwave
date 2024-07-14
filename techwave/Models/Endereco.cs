using System.ComponentModel.DataAnnotations;

namespace techwave.Models
    {
        public class Endereco
        {
            public int Id { get; set; }

            [Required]
            public string Rua { get; set; }

            [Required]
            public string Numero { get; set; }

            [Required]
            public string CEP { get; set; }

            [Required]
            public string Cidade { get; set; }

            [Required]
            public string Estado { get; set; }
          
            
        }
    }

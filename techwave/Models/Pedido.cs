using techwave.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace techwave.Models
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }

        [Required]
        public DateTime DataPedido { get; set; }

        [Required]
        public string Status { get; set; }

        public ICollection<PedidoProduto> PedidoProdutos { get; set; } = new List<PedidoProduto>();


    }
}

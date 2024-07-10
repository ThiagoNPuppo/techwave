using ProjetoFinal.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace techwave.Models
{
    public class PedidoProduto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PedidoId { get; set; }

        [ForeignKey("PedidoId")]
        public Pedido Pedido { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        [Required]
        public int Quantidade { get; set; }
    }
}

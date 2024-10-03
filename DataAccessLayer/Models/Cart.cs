using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models {
    public class CartItem : BaseEntity {
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("CartId")]  
        public int CartId { get; set; }
        public Cart Cart { get; set; }
    }

    public class Cart : BaseEntity {
        public virtual ICollection<CartItem>? Items { get; set; }

    }

}

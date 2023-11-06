using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFirstWebApi.Data
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double unitPrice { get; set; }
        public int Discount { get; set; }
        public int? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public ProductType Type { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public Product()
        {
            OrderDetails = new List<OrderDetail>();
        }
    }
}

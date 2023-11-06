using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFirstWebApi.Data
{
    [Table("ProductType")]
    public class ProductType
    {
        [Key]
        public int TypeId { get; set; }
        [Required]
        public string NameType { get; set; }

        public virtual ICollection<Product>  Products { get; set; }
    }
}

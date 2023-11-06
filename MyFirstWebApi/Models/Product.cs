namespace MyFirstWebApi.Models
{
    public class ProductVM //view model
    {
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public int Discount { get; set; }
        public int TypeId { get; set; }
    }
    public class Product : ProductVM
    {
        public Guid ProductId { get; set; }  
    }

    public class ProductModel
    {
        public Guid ProductId { get; set;}
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public int Discount { get; set; }
        public string TypeName { get; set; }
    }
}

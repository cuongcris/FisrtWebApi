namespace MyFirstWebApi.Data
{
    public class OrderDetail
    {
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public int Amount { get; set; }
        public double UnitPrice { get; set; } // unit price current
        public int Discount { get; set; }    // discount current

        //relationship
        public Order Order { get; set; }
        public Product Product { get; set; }

    }
}

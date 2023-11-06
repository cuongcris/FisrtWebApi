namespace MyFirstWebApi.Data
{
    public enum OrderStatus
    {
        New  = 0 , Payment = 1 , Complete = 2, Cancel = -1
    }
    public class Order
    {
        public Guid OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ShipDate { get; set; }
        public OrderStatus  OrderStatus { get; set; }
        public string Customer { get; set; }
        public string Address { get; set; }
        
        public ICollection<OrderDetail> OrderDetails { get; set; } 

        public Order()
        {
            OrderDetails = new List<OrderDetail>(); //alway create 1 empty list when list = null
        }
    }
}

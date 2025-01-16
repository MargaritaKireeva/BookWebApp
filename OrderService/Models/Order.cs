namespace OrderService.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CartId { get; set; } 
        public List<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

        
    }
}

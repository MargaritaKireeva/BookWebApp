using System.ComponentModel.DataAnnotations.Schema;

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

    public static class OrderIdGenerator
    {
       // private static int _currentId = 0;

        public static int GetNextId(int _currentId)
        {
            return ++_currentId; 
        }
    }
}

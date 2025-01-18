namespace OrderService.Models
{
    public class OutboxMessage
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Status { get; set; } // e.g., Pending, Sent, Failed
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

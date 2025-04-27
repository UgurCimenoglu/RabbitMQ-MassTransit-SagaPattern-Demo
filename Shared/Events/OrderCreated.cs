namespace Shared.Events
{
    public class OrderCreated
    {
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
    }
}

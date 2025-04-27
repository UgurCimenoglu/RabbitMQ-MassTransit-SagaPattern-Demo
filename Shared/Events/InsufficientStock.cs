namespace Shared.Events
{
    public class InsufficientStock
    {
        public Guid OrderId { get; set; }
        public string Reason { get; set; }
    }
}

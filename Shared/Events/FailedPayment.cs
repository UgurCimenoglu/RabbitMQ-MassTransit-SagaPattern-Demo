namespace Shared.Events
{
    public class FailedPayment
    {
        public Guid OrderId { get; set; }
        public string Error { get; set; }
    }
}

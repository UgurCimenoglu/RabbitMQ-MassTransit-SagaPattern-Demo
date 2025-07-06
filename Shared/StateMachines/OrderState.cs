using MassTransit;
using System.ComponentModel.DataAnnotations;

namespace Shared.StateMachines
{
    public class OrderState : SagaStateMachineInstance
    {
        [Key]
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public byte[] RowVersion { get; set; }

    }
}

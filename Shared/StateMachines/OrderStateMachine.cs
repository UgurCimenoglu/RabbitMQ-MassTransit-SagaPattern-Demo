using MassTransit;
using Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public State WaitingForStockControl { get; private set; }
        public State WaitingForPayment { get; private set; }
        public State Completed { get; private set; }
        public State Canceled { get; private set; }

        public Event<OrderCreated> OrderCreated { get; private set; }
        public Event<ConfirmedStock> ConfirmedStock { get; private set; }
        public Event<InsufficientStock> InsufficientStock { get; private set; }
        public Event<SuccessfullyPayment> SuccessfullyPayment { get; private set; }
        public Event<FailedPayment> FailedPayment { get; private set; }

        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);
            Event(() => OrderCreated, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => ConfirmedStock, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => InsufficientStock, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => SuccessfullyPayment, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => FailedPayment, x => x.CorrelateById(context => context.Message.OrderId));

            Initially(

                When(OrderCreated)
                .Then(context =>
                {
                    context.Instance.OrderId = context.Data.OrderId;
                    Console.WriteLine($"[Saga] Sipariş alındı: {context.Data.OrderId}");
                })
                .TransitionTo(WaitingForStockControl)
                );

            During(WaitingForStockControl,

                When(ConfirmedStock)
                .Then(context => Console.WriteLine($"[Saga] Stok onaylandı: {context.Data.OrderId}"))
                .TransitionTo(WaitingForPayment),

                When(InsufficientStock)
                .Then(context => Console.WriteLine($"[Saga] Stok yetersiz: {context.Data.OrderId}"))
                .TransitionTo(Canceled)
                .Finalize()
                );

            During(WaitingForPayment,

                When(SuccessfullyPayment)
                .Then(context => Console.WriteLine($"[Saga] Ödeme başarılı: {context.Data.OrderId}"))
                .TransitionTo(Completed)
                    .Finalize(),

                When(FailedPayment)
                    .Then(context => Console.WriteLine($"[Saga] Ödeme başarısız: {context.Data.OrderId}"))
                    .TransitionTo(Canceled)
                    .Finalize()
                );

            SetCompletedWhenFinalized();
        }

    }
}

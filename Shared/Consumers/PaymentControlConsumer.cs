using MassTransit;
using Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Consumers
{
    public class PaymentControlConsumer : IConsumer<ConfirmedStock>
    {
        private readonly Random _random = new();
        public async Task Consume(ConsumeContext<ConfirmedStock> context)
        {
            Console.WriteLine($"[Payment] Stok onaylandı: {context.Message.OrderId}");

            if (_random.Next(1, 10) <= 8)
            {
                Console.WriteLine($"[Payment] Ödeme BAŞARILI: {context.Message.OrderId}");
                await context.Publish(new SuccessfullyPayment
                {
                    OrderId = context.Message.OrderId
                });
            }
            else
            {
                Console.WriteLine($"[Payment] Ödeme BAŞARISIZ: {context.Message.OrderId}");
                await context.Publish(new FailedPayment
                {
                    OrderId = context.Message.OrderId
                });
            }

        }
    }
}

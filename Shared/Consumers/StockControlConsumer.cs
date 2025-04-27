using MassTransit;
using Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Consumers
{
    public class StockControlConsumer : IConsumer<OrderCreated>
    {
        private readonly Random _random = new Random();
        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            Console.WriteLine($"[Stock] Sipariş geldi: {context.Message.OrderId}");

            if (_random.Next(1, 10) <= 7)
            {
                Console.WriteLine($"[Stock] Stok YETERLİ: {context.Message.OrderId}");

                await context.Publish(new ConfirmedStock { OrderId = context.Message.OrderId });
            }
            else
            {
                Console.WriteLine($"[Stock] Stok YETERSİZ: {context.Message.OrderId}");
                await context.Publish(new InsufficientStock { OrderId = context.Message.OrderId });
            }
        }
    }
}

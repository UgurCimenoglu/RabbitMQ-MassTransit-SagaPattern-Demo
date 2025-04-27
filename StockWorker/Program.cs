using MassTransit;
using Shared.Consumers;

namespace StockWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<StockControlConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ReceiveEndpoint("stock-service", e =>
                    {
                        e.ConfigureConsumer<StockControlConsumer>(context);
                    });
                });
            });


            var host = builder.Build();
            host.Run();
        }
    }
}
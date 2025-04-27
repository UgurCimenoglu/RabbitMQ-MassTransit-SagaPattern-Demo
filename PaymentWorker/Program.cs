using MassTransit;
using Shared.Consumers;

namespace PaymentWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<PaymentControlConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ReceiveEndpoint("payment-service", e =>
                    {
                        e.ConfigureConsumer<PaymentControlConsumer>(context);
                    });
                });
            });

            var host = builder.Build();
            host.Run();
        }
    }
}
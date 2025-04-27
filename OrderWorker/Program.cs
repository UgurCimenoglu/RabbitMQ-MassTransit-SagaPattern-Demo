using MassTransit;
using Shared.StateMachines;

namespace OrderWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddMassTransit(x =>
            {
                x.AddSagaStateMachine<OrderStateMachine, OrderState>()
                .InMemoryRepository();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });

            var host = builder.Build();
            host.Run();
        }
    }
}
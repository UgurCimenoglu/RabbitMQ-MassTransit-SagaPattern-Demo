using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderWorker.Context;
using Shared.StateMachines;

namespace OrderWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddDbContext<OrderDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("OrderDb"));
            });

            builder.Services.AddMassTransit(x =>
            {
                x.AddSagaStateMachine<OrderStateMachine, OrderState>()
                .EntityFrameworkRepository(r =>
                {
                    r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                    r.AddDbContext<DbContext, OrderDbContext>((provider, optionsBuilder) =>
                    {
                        optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("OrderDb"));
                    });
                });

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
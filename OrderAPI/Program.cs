
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;
namespace OrderAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.MapPost("/api/orders", async (IPublishEndpoint publishEndpoint) =>
            {
                var orderId = Guid.NewGuid();
                orderId = Guid.Parse("145AB339-E107-4ECA-BC80-D70620F898C6");
                var amount = new Random().Next(100, 1000);

                var createdOrder = new OrderCreated
                {
                    OrderId = orderId,
                    Amount = amount,
                };

                await publishEndpoint.Publish(createdOrder);
                return Results.Ok(new { Message = "Order Created!", orderId = orderId });
            });

            app.Run();
        }
    }
}

using FreakyFashionServices.OrderProcessor.Data;
using FreakyFashionServices.OrderProcessor.Models.Domain;
using FreakyFashionServices.OrderProcessor.Models.DTO;
using FreakyFashionServices.OrderProcessor.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using static System.Console;

var context = new ApplicationContext();

var factory = new ConnectionFactory
{
    Uri = new Uri("amqp://guest:guest@localhost:5672")
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(
   queue: "order",
   durable: true,
   exclusive: false,
   autoDelete: false,
   arguments: null);

var consumer = new EventingBasicConsumer(channel);

var orderManager = new OrderManager(context);

consumer.Received += (sender, e) => {

    WriteLine("Processing order...");

    var body = e.Body.ToArray();
    var json = Encoding.UTF8.GetString(body);

    var dto = JsonSerializer.Deserialize<OrderDto>(json);

    var order = new Order { 
        Id = dto.Id,
        Customer = dto.Customer,
        OrderLines = dto.OrderLines.Select(x => new Order.OrderLine
        {
            ProductId = x.ProductId,
            Quantity = x.Quantity
        }).ToList()
        };

    orderManager.RegisterOrder(order);

    WriteLine($"Added order: {order.Id}");
};

channel.BasicConsume(
   queue: "order",
   autoAck: true,
   consumer: consumer);

WriteLine(" Press [ENTER] to exit.");
ReadLine();
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class RabbitMqWorker : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Setup RabbitMQ connection, channel, etc.
        ConnectionFactory factory = new ConnectionFactory
        {
            Uri = new Uri("amqp://guest:guest@myrabbit:5672"),
            DispatchConsumersAsync = true // Important for async processing
        };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            string exchangename = "demoexchangefanout";
            string routingkey = "demo-routing-key";
            string queuename = "demoqueue_r1";

            channel.ExchangeDeclare(exchangename, ExchangeType.Fanout);
            channel.QueueDeclare(queuename, false, false, false, null);
            channel.QueueBind(queuename, exchangename, routingkey, null);
            channel.BasicQos(0, 1, false);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (sender, args) =>
            {
                var body = args.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Message Received: {message}");

                await Task.Delay(TimeSpan.FromSeconds(6)); // Simulate processing time

                channel.BasicAck(args.DeliveryTag, false);
            };

            channel.BasicConsume(queuename, false, consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000); // Wait for 1 second
            }
        }
    }
}
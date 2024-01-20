using RabbitMQ.Client;
using System;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Tasks;

ConnectionFactory factory = new();
factory.Uri = new Uri("amqp://guest:guest@myrabbit:5672");
factory.ClientProvidedName = "Rabbit receiver App2";

IConnection conn = factory.CreateConnection();

IModel channel = conn.CreateModel();

string exchangename = "demoexchangefanout";

string routingkey = "demo-routing-key";
string queuename = "demoqueue_r1";

channel.ExchangeDeclare(exchangename, ExchangeType.Fanout);
channel.QueueDeclare(queuename, false, false, false, null);
channel.QueueBind(queuename, exchangename, routingkey, null);
channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, args) =>
{
    Task.Delay(TimeSpan.FromSeconds(6)).Wait();
    var body = args.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Message Received:{message}");

    channel.BasicAck(args.DeliveryTag, false);
};

string consumerTag = channel.BasicConsume(queuename, false, consumer);
Console.ReadLine();

channel.BasicCancel(consumerTag);

channel.Close();

conn.Close();

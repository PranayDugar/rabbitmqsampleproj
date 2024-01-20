using RabbitMQ.Client;
using System;
using System.Text;

ConnectionFactory factory = new();

factory.Uri = new Uri("amqp://guest:guest@myrabbit:5672");

factory.ClientProvidedName = "Rabbit Sender App";

IConnection conn = factory.CreateConnection();

IModel channel = conn.CreateModel();

string exchangename = "demoexchangefanout";
string routingkey = "demo-routing-key";
string queuename = "demoqueue";

channel.ExchangeDeclare(exchangename, ExchangeType.Fanout);
channel.QueueDeclare(queuename, false, false, false, null);
channel.QueueBind(queuename, exchangename, routingkey, null);

byte[] messageBodyBytes = Encoding.UTF8.GetBytes("Hello welcome to the rabbitmq34");
channel.BasicPublish(exchangename, routingkey, null, messageBodyBytes);

channel.Close();
conn.Close();
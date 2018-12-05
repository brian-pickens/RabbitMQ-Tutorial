using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

class RecieveLogs
{
    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
        var queue = channel.QueueDeclare();

        channel.ExchangeDeclare(exchange: "logs", type: "fanout");
        channel.QueueBind(queue: queue.QueueName,
                            exchange: "logs",
                            routingKey: "");

        Console.WriteLine(" [*] Waiting for logs.");

        var consumer = new EventingBasicConsumer(channel);            
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);
        };

        channel.BasicConsume(queue: queue.QueueName,
                                autoAck: true,
                                consumer: consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
        }
    }
}
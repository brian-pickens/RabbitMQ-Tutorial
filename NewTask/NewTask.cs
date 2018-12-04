using System;
using System.Text;
using RabbitMQ.Client;

class NewTask
{
    public static void Main(params string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost"};
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.BasicQos(0, 0, false);
            channel.QueueDeclare(queue: "task_queue",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            var message = GetMessage(args);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "",
                                    routingKey: "hello",
                                    basicProperties: null,
                                    body: body);

            Console.WriteLine( "[x] Sent {0}", message);
        }
    }

    private static string GetMessage(string[] args)
    {
        return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
    }
}

using System;
using System.Text;
using RabbitMQ.Client;

class Program
{
    public static void Main(params string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost"};
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {

            channel.ExchangeDeclare(exchange: "logs", type: "fanout");

            var message = GetMessage(args);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "logs",
                                 routingKey: "",
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

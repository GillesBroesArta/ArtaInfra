using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Arta.Infrastructure.Event
{
    public static class RabbitMQ
    {
        //For the moment, no exchange or routing key is used. The event is just published to a named queue 
        //(for more info see https://www.rabbitmq.com/tutorials/tutorial-four-dotnet.html and https://www.rabbitmq.com/tutorials/tutorial-five-dotnet.html )
        //Maybe it would be interesting to use a topic exchange and use a routing key identifying the kind of event sent.
        public static void SendEvent<T>(T anEvent, string host, string queue) where T : class
        {
            var factory = new ConnectionFactory() { HostName = host };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(anEvent));
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.QueueDeclare(queue: queue,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    channel.BasicPublish(exchange: "",
                        routingKey: "",
                        basicProperties: properties,
                        body: body);
                }
            }
        }
    }
}

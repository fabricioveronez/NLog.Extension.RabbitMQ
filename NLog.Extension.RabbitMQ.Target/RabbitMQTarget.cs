using NLog.Config;
using NLog.Targets;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLog.Extension.RabbitMQ.Target
{
    [Target("RabbitMQ")]
    public class RabbitMQTarget : TargetWithLayout
    {
        public RabbitMQTarget()
        {
            this.HostName = "localhost";
            this.Port = 5672;
        }

        public string HostName { get; set; }

        [RequiredParameter]
        public string UserName { get; set; }

        [RequiredParameter]
        public string Password { get; set; }

        public int Port { get; set; }

        public string Queue { get; set; }

        public string Exchange { get; set; }

        public string RoutingKey { get; set; }

        protected override void Write(LogEventInfo logEvent)
        {
            string logMessage = this.Layout.Render(logEvent);

            var factory = new ConnectionFactory() { HostName = this.HostName, Port = this.Port, UserName = this.UserName, Password = this.Password };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: this.Queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                    var body = Encoding.UTF8.GetBytes(logMessage);

                    channel.BasicPublish(exchange: this.Exchange,
                                         routingKey: this.RoutingKey,
                                         basicProperties: null,
                                         body: body);
                }
            }
        }
    }
}

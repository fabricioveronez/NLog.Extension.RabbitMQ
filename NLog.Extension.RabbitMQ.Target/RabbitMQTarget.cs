using NLog.Common;
using NLog.Config;
using NLog.Targets;
using RabbitMQ.Client;
using System;
using System.Text;

namespace NLog.Extension.RabbitMQ.Target
{
    [Target("RabbitMQ")]
    public class RabbitMQTarget : TargetWithLayout
    {
        ConnectionFactory factory;
        private IConnection connection;

        public RabbitMQTarget()
        {
            this.HostName = "localhost";
            this.Port = 5672;
            this.Exchange = "";
            this.RoutingKey = "";
            this.VirtualHost = "/";
        }

        protected override void InitializeTarget()
        {

            try
            {
                factory = new ConnectionFactory() { HostName = this.HostName, VirtualHost = this.VirtualHost, Port = this.Port, UserName = this.UserName, Password = this.Password };
                connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                InternalLogger.Error($"Could not setup NLog target {GetType().Name} - {ex.ToString()}");
            }

            base.InitializeTarget();
        }

        public string HostName { get; set; }

        [RequiredParameter]
        public string UserName { get; set; }

        [RequiredParameter]
        public string Password { get; set; }

        public int Port { get; set; }

        [RequiredParameter]
        public string Exchange { get; set; }

        [RequiredParameter]
        public string RoutingKey { get; set; }

        public string VirtualHost { get; set; }

        protected override void CloseTarget()
        {
            this.connection?.Close();

            base.CloseTarget();
        }
        protected override void Write(LogEventInfo logEvent)
        {
            string logMessage = this.Layout.Render(logEvent);

            try
            {
                using (var channel = connection.CreateModel())
                {
                    var body = Encoding.UTF8.GetBytes(logMessage);

                    IBasicProperties props = channel.CreateBasicProperties();
                    props.ContentType = "text/plain";
                    props.DeliveryMode = 2;

                    channel.BasicPublish(exchange: this.Exchange,
                                         routingKey: this.RoutingKey,
                                         basicProperties: null,
                                         body: body);
                }
            }
            catch (Exception ex)
            {
                InternalLogger.Error("{ Could not send to RabbitMQ: {0} Log: {1} }", ex.ToString(), logMessage);
            }
        }
    }
}

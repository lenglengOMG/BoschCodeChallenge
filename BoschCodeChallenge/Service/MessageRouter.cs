using System;

namespace BoschCodeChallenge
{
    /// <summary>
    /// Can be an independent service to route messages between machines and MachineManager
    /// </summary>
    public class MessageRouter
    { 
        //private readonly RabbitMQConnection _connection;
        //private readonly RabbitMQConfig _config;

        private readonly Lazy<MessageRouter> _instance = new Lazy<MessageRouter>(() => new MessageRouter());

        private MessageRouter()
        {
            // Initialize RabbitMQ connection and configuration
        }

        public static MessageRouter Instance
        {
            get
            {
                return Instance;
            }
        }

        public void SendInstruciton(string message)
        {
            // Simulate sending a message to machine via RabbitMQ
        }

        public void SubScribeStatus(string queueName, string routingKey, Action<string> onReceived)
        {
            // Simulate subscribing to status messages from machines via RabbitMQ
        }
    }
}

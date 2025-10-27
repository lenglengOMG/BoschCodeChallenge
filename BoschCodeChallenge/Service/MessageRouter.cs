using System;

namespace BoschCodeChallenge
{
    /// <summary>
    /// An independent service to route messages between machines and MachineManager
    /// </summary>
    public class MessageRouter
    {
        private readonly Lazy<MessageRouter> _instance = new Lazy<MessageRouter>(() => new MessageRouter());

        private MessageRouter()
        {
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

        public void SendStatus(string message)
        {
            // Simulate sending a status message from machine via RabbitMQ
        }

        public void SendPart(string message)
        {
            // Simulate sending a part message from machine via RabbitMQ
        }

        public void SubScribeInstruction(string queueName, string routingKey, Action<string> onReceived)
        {
            // Simulate subscribing to instruction messages to machines via RabbitMQ
        }

        public void SubScribeStatus(string queueName, string routingKey, Action<string> onReceived)
        {
            // Simulate subscribing to status messages from machines via RabbitMQ
        }

        public void SubScribePart(string queueName, string routingKey, Action<string> onReceived)
        {
            // Simulate subscribing to part messages from machines via RabbitMQ
        }
    }
}

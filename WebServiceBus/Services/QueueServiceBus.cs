using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebServiceBus.Services
{
    public class QueueServiceBus : IQueueServiceBus
    {
        private readonly IConfiguration config;

        public QueueServiceBus(IConfiguration config)
        {
            this.config = config;
        }
        public async Task SendMessageAsync<T>(T messageObj, string queueName)
        {
            string connectionString = config["QueueBus:servicebusconn"];
            QueueClient queueClient = new QueueClient(connectionString, queueName);
            string messageStr = JsonSerializer.Serialize(messageObj);
            var message = new Message(Encoding.UTF8.GetBytes(messageStr));

            await queueClient.SendAsync(message);
        }
    }
}

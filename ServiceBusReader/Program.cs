using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using ServiceBusShared.Models;

/// <summary>
/// Azure service bus receiver.
/// You need to have an account on azure to create service bus and a queue.
/// Service bus connection string and queue name are stored in secrets.json file, 
/// this has to be created for this project.
/// </summary>
namespace ServiceBusReader
{
    class Program
    {
        private static IConfigurationRoot config;
        private static QueueClient queueClient;

        static void Main(string[] args)
        {
            InitializeConfiguration();
            //servicebusconn and queuename are stored in secrets.json file
            queueClient = new QueueClient(config["QueueBus:servicebusconn"], config["QueueBus:queuename"]);
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceiveHandler)
            {
                 MaxConcurrentCalls = 1,
                 AutoComplete = false
            };

            queueClient.RegisterMessageHandler(MessageHanderAsync, messageHandlerOptions);
            Console.ReadLine();
            queueClient.CloseAsync();
        }
        /// <summary>
        /// Initialize configuration
        /// </summary>
        private static void InitializeConfiguration()
        {
            var builder = new ConfigurationBuilder().AddUserSecrets<Program>();
            config = builder.Build();
        }
        /// <summary>
        /// Handle messages from the service bus
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private static async Task MessageHanderAsync(Message message, CancellationToken token)
        {
            string messageJson = Encoding.UTF8.GetString(message.Body);
            Employee emp = JsonSerializer.Deserialize<Employee>(messageJson);

            Console.WriteLine(emp);
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }
        /// <summary>
        /// Handle exception encountered while handling messages
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private static Task ExceptionReceiveHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Exception : {arg.Exception}");
            return Task.CompletedTask;
        }
    }    
}

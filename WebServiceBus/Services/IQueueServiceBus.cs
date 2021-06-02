using System.Threading.Tasks;

namespace WebServiceBus.Services
{
    public interface IQueueServiceBus
    {
        Task SendMessageAsync<T>(T messageObj, string queueName);
    }
}
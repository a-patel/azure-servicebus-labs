using System.Threading.Tasks;

namespace AzureServiceBusLabs.Api.Services
{
    public interface IServiceBusSender
    {
        Task<bool> Send(string messageContent);
    }
}
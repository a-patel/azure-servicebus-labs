using System.Threading.Tasks;

namespace AzureServiceBusLabs.Api.Services
{
    public interface IServiceBusReceiver
    {
        Task<string> Receive();
    }
}
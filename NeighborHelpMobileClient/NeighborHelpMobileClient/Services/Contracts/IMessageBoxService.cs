using System.Threading.Tasks;

namespace NeighborHelpMobileClient.Services.Contracts
{
    public interface IMessageBoxService
    {
        Task<bool> ConfirmAction(string title, string message);
    }
}

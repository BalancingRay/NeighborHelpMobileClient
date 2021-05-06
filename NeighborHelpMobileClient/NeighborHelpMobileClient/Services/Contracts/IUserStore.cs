using NeighborHelpModels.Models;
using System.Threading.Tasks;

namespace NeighborHelpMobileClient.Services.Contracts
{
    public interface IUserStore : IDataStore<User>
    {
        Task<bool> LoginAsync(string login, string password);

        Task<User> GetCurrentUserAsync();
    }
}

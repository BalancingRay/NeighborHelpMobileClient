using NeighborHelpModels.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NeighborHelpMobileClient.Services.Contracts
{
    public interface IOrderStore : IDataStore<Order>
    {
        Task<IEnumerable<Order>> GetItemsByUserIdAsync(int userId);
    }
}

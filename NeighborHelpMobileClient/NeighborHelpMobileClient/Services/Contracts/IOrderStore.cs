using NeighborHelpModels.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NeighborHelpMobileClient.Services.Contracts
{
    public interface IOrderStore : IDataStore<Order>
    {
        Task<IEnumerable<Order>> GetItemsByUserIdAsync(string userId);

        Task<IEnumerable<Order>> GetCurrentUserItemsAsync();

        Task<bool> UseOrder(Order order);
    }
}

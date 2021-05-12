using NeighborHelpAPI.Consts;
using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpModels.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NeighborHelpMobileClient.Services
{
    public class OrderStore : IOrderStore
    {
        private ServerConnecter connecter;

        public OrderStore()
        {
            connecter = new ServerConnecter();
        }

        public async Task<bool> AddItemAsync(Order item)
        {
            try
            {
                var result = await connecter.Post<Order>(PathConst.ADD_ORDER_PATH, item, true);

                bool isNotEmpty = !string.IsNullOrEmpty(result);
                return isNotEmpty;
            }
            catch (Exception exc)
            {
                Debug.Fail(exc.Message);
                return false;
            }
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();

            //TODO fix return
        }

        public async Task<Order> GetItemAsync(string id)
        {
            try
            {
                return await connecter.Get<Order>($"{PathConst.GET_ORDER_PATH}/{id}");
            }
            catch (Exception exc)
            {
                Debug.Fail(exc.Message);
                return null;
            }
        }

        public async Task<IEnumerable<Order>> GetItemsAsync(bool forceRefresh = false)
        {
            try
            {
                return await connecter.GetList<Order>(PathConst.GET_ORDERS_PATH);
            }
            catch (Exception exc)
            {
                Debug.Fail(exc.Message);
                return new Order[0];
            }
        }

        public async Task<IEnumerable<Order>> GetItemsByUserIdAsync(string userId)
        {
            try
            {
                return await connecter.GetList<Order>($"{PathConst.GET_ORDERS_BY_USER_PATH}/{userId}");
            }
            catch (Exception exc)
            {
                Debug.Fail(exc.Message);
                return new Order[0];
            }
        }

        public async Task<IEnumerable<Order>> GetCurrentUserItemsAsync()
        {
            try
            {
                var userId = Xamarin.Forms.DependencyService.Get<IConnectorProvider>()?.GetToken()?.UserId;
                return await GetItemsByUserIdAsync(userId);
            }
            catch(Exception exc)
            {
                Debug.Fail(exc.Message);
                return new Order[0];
            }
        }

        public async Task<bool> UseOrder(Order order)
        {
            try
            {
                var result = await connecter.Put<Order>(PathConst.RESPONCE_ORDER_PATH, order, true);

                bool isNotEmpty = !string.IsNullOrEmpty(result);
                return isNotEmpty;
            }
            catch (Exception exc)
            {
                Debug.Fail(exc.Message);
                return false;
            }
        }

        public async Task<bool> UpdateItemAsync(Order item)
        {
            try
            {
                var result = await connecter.Put<Order>(PathConst.PUT_ORDER_PATH, item, true);

                bool isNotEmpty = !string.IsNullOrEmpty(result);
                return isNotEmpty;
            }
            catch (Exception exc)
            {
                Debug.Fail(exc.Message);
                return false;
            }
        }
    }
}

using NeighborHelpAPI.Consts;
using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpModels.Models;
using System;
using System.Collections.Generic;
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
                await connecter.Post<Order>(PathConst.ADD_ORDER_PATH, item, true);
            }
            catch (Exception exc)
            {
                throw (exc);
            }

            //TODO fix return
            return true;
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
                throw (exc);
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
                throw (exc);
            }
        }

        public async Task<IEnumerable<Order>> GetItemsByUserIdAsync(int userId)
        {
            try
            {
                return await connecter.GetList<Order>($"{PathConst.GET_ORDERS_BY_USER_PATH}/{userId}");
            }
            catch (Exception exc)
            {
                throw (exc);
            }
        }

        public async Task<bool> UpdateItemAsync(Order item)
        {
            try
            {
                await connecter.Put<Order>(PathConst.PUT_ORDER_PATH, item, true);
            }
            catch (Exception exc)
            {
                throw (exc);
            }

            //TODO fix return
            return true;
        }
    }
}

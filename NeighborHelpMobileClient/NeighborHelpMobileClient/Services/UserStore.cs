using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpModels.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NeighborHelpModels.ControllersModel;
using Newtonsoft.Json;
using Xamarin.Forms;
using NeighborHelpAPI.Consts;
using System.Diagnostics;

namespace NeighborHelpMobileClient.Services
{
    public class UserStore : IUserStore
    {
        private ServerRESTConnector connector;

        public UserStore()
        {
            connector = new ServerRESTConnector();
        }

        public async Task<bool> LoginAsync(string login, string password)
        {
            try
            {
                var data = new AuthentificateData() { Login = login, Password = password };
                var result = await connector.Post(PathConst.LOGIN_BY_JWT_PATH, data);

                var token = JsonConvert.DeserializeObject<AuthentificateToken>(result);

                if (token?.Token != null)
                {
                    DependencyService.Get<IConnectorProvider>().UpdateToken(token);
                    return true;
                }

                return false;
            }
            catch (Exception exc)
            {
                Debug.Fail(exc.Message);
                return false;
            }
        }

        public void Unlogin()
        {
            DependencyService.Get<IConnectorProvider>().UpdateToken(new AuthentificateToken());
        }

        public async Task<bool> AddItemAsync(User item)
        {
            try
            {
                var result = await connector.Post<User>(PathConst.ADD_USER_PATH, item);

                bool isResultNotEmpty = !string.IsNullOrEmpty(result);
                return isResultNotEmpty;
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
        }

        public async Task<User> GetItemAsync(string id)
        {
            try
            {
                return await connector.Get<User>($"{PathConst.GET_USER_PATH}/{id}", true);
            }
            catch (Exception exc)
            {
                Debug.Fail(exc.Message);
                return null;
            }
        }

        public async Task<User> GetCurrentUserAsync()
        {
            try
            {
                return await connector.Get<User>(PathConst.CURRENT_USER_PATH, true);
            }
            catch (Exception exc)
            {
                Debug.Fail(exc.Message);
                return null;
            }
        }

        public async Task<IEnumerable<User>> GetItemsAsync(bool forceRefresh = false)
        {
            try
            {
                return await connector.GetList<User>(PathConst.GET_USERS_PATH, true);
            }
            catch (Exception exc)
            {
                Debug.Fail(exc.Message);
                return new User[0];
            }
        }

        public async Task<bool> UpdateItemAsync(User item)
        {
            try
            {
                var result = await connector.Put<User>(PathConst.PUT_USER_PATH, item, true);

                bool isResultNotEmpty = !string.IsNullOrEmpty(result);
                return isResultNotEmpty;
            }
            catch (Exception exc)
            {
                Debug.Fail(exc.Message);
                return false;
            }
        }
    }
}

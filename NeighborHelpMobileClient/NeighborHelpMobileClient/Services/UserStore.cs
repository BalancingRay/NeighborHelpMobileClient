using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpModels.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NeighborHelpModels.ControllersModel;
using Newtonsoft.Json;
using Xamarin.Forms;
using NeighborHelpAPI.Consts;

namespace NeighborHelpMobileClient.Services
{
    public class UserStore : IUserStore
    {
        private ServerConnecter connecter;

        public UserStore()
        {
            connecter = new ServerConnecter();
        }

        public async Task<bool> LoginAsync(string login, string password)
        {
            var data = new AuthentificateData() { Login = login, Password = password };
            var result = await connecter.Post(PathConst.LOGIN_BY_JWT_PATH, data);

            string token = JsonConvert.DeserializeObject<AuthentificateToken>(result)?.Token;

            if (!string.IsNullOrEmpty(token))
            {  
                DependencyService.Get<IConnectorProvider>().UpdateToken(token);
                return true;
            }

            return false;
        }

        public async Task<bool> AddItemAsync(User item)
        {
            await connecter.Post<User>(PathConst.ADD_USER_PATH, item);

            //TODO fix result
            return true;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetItemAsync(string id)
        {
            return await connecter.Get<User>($"{PathConst.GET_USER_PATH}/{id}", true);
        }

        public async Task<User> GetCurrentUserAsync()
        {
            return await connecter.Get<User>(PathConst.CURRENT_USER_PATH, true);
        }

        public async Task<IEnumerable<User>> GetItemsAsync(bool forceRefresh = false)
        {
            return await connecter.GetList<User>(PathConst.GET_USERS_PATH, true);
        }

        public async Task<bool> UpdateItemAsync(User item)
        {
            await connecter.Put<User>(PathConst.PUT_USER_PATH, item, true);

            //TODO fix result
            return true;
        }
    }
}

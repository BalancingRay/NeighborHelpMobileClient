using NeighborHelpMobileClient.Services;
using NeighborHelpMobileClient.Services.Contracts;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private string _serverAddress;
        public string ServerAddress
        {
            get => _serverAddress;
            set
            {
                SetProperty(ref _serverAddress, value);

                if (_serverAddress != DependencyService.Get<IConnectorProvider>().GetServerUrl())
                {
                    DependencyService.Get<IConnectorProvider>().UpdateServerUrl(_serverAddress);
                }
            }
        }
    }
}
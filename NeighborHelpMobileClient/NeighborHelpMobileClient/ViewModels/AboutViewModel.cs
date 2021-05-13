using NeighborHelpMobileClient.Services.Contracts;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private string _serverAddress;

        private IConnectorProvider ConnectorProvider => DependencyService.Get<IConnectorProvider>();

        public AboutViewModel()
        {
            ServerAddress = ConnectorProvider.GetServerUrl();
        }

        public string ServerAddress
        {
            get => _serverAddress;
            set
            {
                SetProperty(ref _serverAddress, value);

                if (_serverAddress != ConnectorProvider.GetServerUrl())
                {
                    ConnectorProvider.UpdateServerUrl(_serverAddress);
                }
            }
        }
    }
}
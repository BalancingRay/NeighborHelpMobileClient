using NeighborHelpMobileClient.Services;
using NeighborHelpMobileClient.Services.Contracts;
using Xamarin.Forms;

namespace NeighborHelpMobileClient
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<IOrderStore, OrderStore>();
            DependencyService.Register<IUserStore, UserStore>();
            DependencyService.RegisterSingleton<IConnectorProvider>(new AuthenticationTokenProvider());

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

using Client.Administration.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Client.Administration
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider serviceProvider;
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddScoped<ITokenManager, TokenManager>();
            services.AddScoped<IApiClient, ApiClient>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
        }
    }
}

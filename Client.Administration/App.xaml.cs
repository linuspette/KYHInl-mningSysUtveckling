using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Client.Administration.Helpers;
using Client.Administration.Windows.Authentication;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddSingleton<LogIn>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<LogIn>() ?? null!;
            mainWindow.Show();
        }
    }
}

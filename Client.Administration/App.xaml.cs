﻿using Client.Administration.Helpers;
using Client.Administration.MVVM.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace Client.Administration
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? app { get; private set; }
        public App()
        {
            app = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
            {
                services.AddSingleton<MainWindow>();
                services.AddSingleton<NavigationStore>();
            }).Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var navigationStore = app!.Services.GetRequiredService<NavigationStore>();
            navigationStore.CurrentViewModel = new KitchenViewModel(navigationStore);

            await app!.StartAsync();
            var MainWindow = app.Services.GetRequiredService<MainWindow>();
            MainWindow.DataContext = new MainViewModel(navigationStore);
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await app!.StopAsync();
            base.OnExit(e);
        }
    }
}

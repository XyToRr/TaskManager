using System.Configuration;
using System.Data;
using System.Windows;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var serviceProvider = new ServiceCollection();
            ConfigurationService(serviceProvider);
            ServiceProvider = serviceProvider.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetService<LoginWindow>();
            mainWindow.Show();
        }

        private void ConfigurationService(ServiceCollection services)
        {

            services.AddTransient<UserService>();
            services.AddSingleton<TaskManagerClient>();

            services.AddTransient<LoginWindow>();
            services.AddTransient<RegisterWindow>();

        }
    }

}

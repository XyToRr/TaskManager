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
        public static TaskManagerClient Client { get; private set; }
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            var serviceProvider = new ServiceCollection();
            ConfigurationService(serviceProvider);
            ServiceProvider = serviceProvider.BuildServiceProvider();


            Client = new TaskManagerClient();
            await Client.ConnectAsync("127.0.0.1", 5000);

            var mainWindow = ServiceProvider.GetService<LoginWindow>();
            mainWindow.Show();


            //var mainWindow = ServiceProvider.GetService<ProjectsWindow>();
            //mainWindow.Show();

            var mainWindow = ServiceProvider.GetService<ProjectsWindow>();
            mainWindow.Show();


            //var mainWindow = ServiceProvider.GetService<AddProjectWindow>();
            //mainWindow.Show();
        }

        private void ConfigurationService(ServiceCollection services)
        {

            
            //services.AddSingleton<TaskManagerClient>();

            services.AddTransient<LoginWindow>();
            services.AddTransient<RegisterWindow>();
            services.AddTransient<ProjectsWindow>();
            services.AddTransient<AddProjectWindow>();
            
            services.AddTransient<ProjectService>((ServiceProvider) => new ProjectService(Client));
            services.AddTransient<UserService>((ServiceProvider) => new UserService(Client));
            services.AddTransient<TaskService>((ServiceProvider) => new TaskService(Client));
        }
    }

}

using BLL.Services;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskManager.Configuration.UserAuthentificationHelper;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for AddProjectWindow.xaml
    /// </summary>
    public partial class AddProjectWindow : Window
    {
        private readonly ProjectService _projectService;
        private readonly UserService _userService;

        public AddProjectWindow(ProjectService projectService)
        {
            _projectService = projectService;
            InitializeComponent();
        }

        private async void AddProjectButton_Click(object sender, RoutedEventArgs e)
        {
            var proj = new Project
            {
                Name = NameBox.Text,
                
                Description = DescriptionBox.Text,
               
            };


            _projectService.ProjectCreate(proj);
            var projWindow = App.ServiceProvider.GetService<ProjectsWindow>();
            projWindow.Show();
            await Dispatcher.BeginInvoke(() => this.Close());


        }

        private async void Window_Closed(object sender, EventArgs e)
        {
            var projWindow = App.ServiceProvider.GetService<ProjectsWindow>();
            projWindow.Show();
            await Dispatcher.BeginInvoke(() => this.Close());
        }
    }
}

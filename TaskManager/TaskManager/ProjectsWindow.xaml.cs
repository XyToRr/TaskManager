using BLL.Services;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for ProjectsWindow.xaml
    /// </summary>
    public partial class ProjectsWindow : Window
    {
        private ObservableCollection<Project> Projects;
        private ProjectService _projectService;
        public ProjectsWindow(ProjectService projectService)
        {
            InitializeComponent();
            _projectService = projectService;
            _projectService.AddProject += OnProjectListReceived;
            _projectService.ProjectListRequest();
        }
        private async void AddProject_Click(object sender, RoutedEventArgs e)
        {            
            var addProjectWindow = App.ServiceProvider.GetService<AddProjectWindow>();
            addProjectWindow.ShowDialog();

        }

        private async void OnProjectListReceived(List<Project> projectList)
        {
            foreach (var project in projectList)
            {
                Projects.Add(project);
            }
        }

        private void ProjectsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

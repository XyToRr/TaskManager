using BLL.Services;
using Domain.Models;
using Domain.Models.ClientModels;
using Domain.Models.Configuration.UserAuthentificationHelper;
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
        private ObservableCollection<ProjectOnClient> Projects;
        private readonly ProjectService _projectService;
        private readonly TaskService _taskService;
        
        public ProjectsWindow(ProjectService projectService, TaskService taskService)
        {

            InitializeComponent();


            _taskService = taskService;
            _projectService = projectService;
            _projectService.AddProject += OnProjectListReceived;
            _projectService.ProjectListRequest();

            Projects = new();
            ProjectsListView.ItemsSource = Projects;
        }
        private async void AddProject_Click(object sender, RoutedEventArgs e)
        {            
            var addProjectWindow = App.ServiceProvider.GetService<AddProjectWindow>();
            addProjectWindow.ShowDialog();

        }

        private async void OnProjectListReceived(List<ProjectOnClient> projectList)
        {
            Projects.Clear();
            foreach (var project in projectList)
            {
                Projects.Add(project);
            }
        }

        

        private async void ProjectsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            var projWindow= App.ServiceProvider.GetService<CurrentProjectWindow>();
            projWindow.Show();

            var proj = (ProjectOnClient)ProjectsListView.SelectedItem;
            var projId = proj.Id;

            ProjectHelper.ProjectId = projId.ToString();
            
            await Dispatcher.BeginInvoke(() => this.Close());


            

        }

        private void UpdateProjectsList_Click(object sender, RoutedEventArgs e)
        {
            

            _projectService.ProjectListRequest();
        }
    }
}

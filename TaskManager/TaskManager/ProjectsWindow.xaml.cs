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
        private ObservableCollection<Project> Projects { get; set; }
        public ProjectsWindow()
        {
            InitializeComponent();
            //Projects = new ObservableCollection<Project>
            //{
            //    new Project { Name = "MainProject", Role = "Owner", Description = "Lorem ipsum Lorem ipsum", TaskCount = 5, CreatedDate = "01.01.2025" },
            //    new Project { Name = "MainProject", Role = "Owner", Description = "Lorem ipsum Lorem ipsum", TaskCount = 5, CreatedDate = "01.01.2025" }
            //};

            //ProjectsListView.ItemsSource = Projects;
        }
        private void AddProject_Click(object sender, RoutedEventArgs e)
        {
            //Projects.Add(new Project
            //{
            //    Name = "NewProject",
            //    Role = "Owner",
            //    Description = "New project description",
            //    TaskCount = 0,
            //    CreatedDate = DateTime.Now.ToString("dd.MM.yyyy")
            //});



            // отут логіка відкриття вікна
            
            var addProjectWindow = App.ServiceProvider.GetService<AddProjectWindow>();
            addProjectWindow.ShowDialog();

        }

        private void ProjectsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
    public class Project
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public int TaskCount { get; set; }
        public string CreatedDate { get; set; }
    }
}

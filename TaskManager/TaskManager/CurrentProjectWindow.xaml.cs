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
    /// Interaction logic for CurrentProjectWindow.xaml
    /// </summary>
    public partial class CurrentProjectWindow : Window
    {
        public TaskViewModel ViewModel { get; set; }
        public CurrentProjectWindow()
        {
            InitializeComponent();
            ViewModel = new TaskViewModel();
            DataContext = ViewModel;

        }







    }




    public class TaskViewModel
    {
        public ObservableCollection<TaskItem> Tasks { get; set; }
        public List<string> Statuses { get; set; }

        public TaskViewModel()
        {
            Statuses = new List<string> { "Active", "Completed", "Pending" };
            Tasks = new ObservableCollection<TaskItem>
        {
            new TaskItem { Name = "Fix Bug #123", CreatedBy = "Nazar Korobchuk", Description = "Fix critical login issue.", Priority = "High", Status = "Active" },
            new TaskItem { Name = "Add Feature X", CreatedBy = "Ivan Petrov", Description = "Implement new dashboard.", Priority = "Medium", Status = "Pending" },
            new TaskItem { Name = "Code Review", CreatedBy = "Dima Ivanov", Description = "Review pull request #45.", Priority = "Low", Status = "Completed" }
        };
        }
    }

    public class TaskItem
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
    }





}

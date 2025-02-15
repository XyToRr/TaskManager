﻿using Microsoft.Extensions.DependencyInjection;
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
            
            ViewModel.Statuses = new List<string> { "Active", "Completed", "Pending" };
            ViewModel.Tasks= new ObservableCollection<TaskItem>
        {
            new TaskItem { Name = "Fix Bug #123", CreatedBy = "Nazar Korobchuk", Description = "Fix critical login issue.", Priority = "High", Status = "Active", Deadline="01.11.25", CreatedAt="05.8.25" },
            new TaskItem { Name = "Add Feature X", CreatedBy = "Ivan Petrov", Description = "Implement new dashboard.", Priority = "Medium", Status = "Pending" , Deadline="01.11.25", CreatedAt="05.8.25" },
            new TaskItem { Name = "Code Review", CreatedBy = "Dima Ivanov", Description = "Review pull request #45.", Priority = "Low", Status = "Completed" , Deadline="01.11.25", CreatedAt="05.8.25" },
            new TaskItem { Name = "Fix Bug #123", CreatedBy = "Nazar Korobchuk", Description = "Fix critical login issue.", Priority = "High", Status = "Active", Deadline="01.11.25", CreatedAt="05.8.25" },
            new TaskItem { Name = "Add Feature X", CreatedBy = "Ivan Petrov", Description = "Implement new dashboard.", Priority = "Medium", Status = "Pending" , Deadline="01.11.25", CreatedAt="05.8.25" },
            new TaskItem { Name = "Code Review", CreatedBy = "Dima Ivanov", Description = "Review pull request #45.", Priority = "Low", Status = "Completed" , Deadline="01.11.25", CreatedAt="05.8.25" },
            new TaskItem { Name = "Fix Bug #123", CreatedBy = "Nazar Korobchuk", Description = "Fix critical login issue.", Priority = "High", Status = "Active", Deadline="01.11.25", CreatedAt="05.8.25" },
            new TaskItem { Name = "Add Feature X", CreatedBy = "Ivan Petrov", Description = "Implement new dashboard.", Priority = "Medium", Status = "Pending" , Deadline="01.11.25", CreatedAt="05.8.25" },
            new TaskItem { Name = "Code Review", CreatedBy = "Dima Ivanov", Description = "Review pull request #45.", Priority = "Low", Status = "Completed" , Deadline="01.11.25", CreatedAt="05.8.25" },
        };

        }

        private void AddUserBtn_Click(object sender, RoutedEventArgs e)
        {
            var addUserWindow= App.ServiceProvider.GetService<AddUserToProjWindow>();
            addUserWindow.ShowDialog();
        }

        private void AddTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            var addTaskWindow = App.ServiceProvider.GetService<AddTaskToProjWindow>();
            addTaskWindow.ShowDialog();
        }




        private void Window_Activated(object sender, EventArgs e)
        {
            // підгрузити всі дані при запуску проги




            //вказати видимість кнопок в залежності від ролі

            AddUserBtn.Visibility = Visibility.Visible;
            AddTaskBtn.Visibility = Visibility.Visible;
        }
    }




    public class TaskViewModel
    {
        public ObservableCollection<TaskItem> Tasks { get; set; }
        public List<string> Statuses { get; set; }

        public TaskViewModel()
        {
            Statuses = new List<string>();
            Tasks = new ObservableCollection<TaskItem>();
        }
    }

    public class TaskItem
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }

        public string CreatedAt { get; set; }
        public string Deadline { get; set; }
    }





}

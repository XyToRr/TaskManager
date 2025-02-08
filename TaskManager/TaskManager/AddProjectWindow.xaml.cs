using BLL.Services;
using Domain.Models;
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
       // private readonly TaskManagerClient _taskManagerClient;
        private readonly UserService _userService;

        public AddProjectWindow(UserService userService)
        {
           // _taskManagerClient = taskManagerClient;
            _userService = userService;
            InitializeComponent();
        }

        private void AddProjectButton_Click(object sender, RoutedEventArgs e)
        {
            var proj = new Project
            {
                Name = NameBox.Text,
                
                Description = DescriptionBox.Text,
               
            };


            _ = ConnectionService.Instance.Client.SendMessageAsync(new Message()
            {
                Content = JsonSerializer.Serialize(proj),
                MessageType = MessageType.ProjectCreationRequest,
                Token = UserAuthentificationHelper.Token

            });

            Close();



        }
    }
}

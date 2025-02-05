using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLL.Services;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly TaskManagerClient _taskManagerClient;
        public LoginWindow(TaskManagerClient taskManagerClient)
        {
            InitializeComponent();
            _taskManagerClient = taskManagerClient;
            Task.Run(() => _taskManagerClient.ConnectAsync("127.0.0.1", 5000));
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
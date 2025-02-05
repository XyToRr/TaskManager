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
using Microsoft.Extensions.DependencyInjection;



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

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = App.ServiceProvider.GetService<RegisterWindow>();
            registerWindow.ShowDialog();

        }

        private async void ClearForm()
        {
            LoginTextBox.Text = string.Empty;
            PasswordTextBox.Text = string.Empty;
        }


        private async Task<bool> ValidateLogin()
        {
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text) || string.IsNullOrWhiteSpace(PasswordTextBox.Text))
            {
                MessageBox.Show("Логін або пароль не може бути порожнім.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearForm();
                return false;
            }

           
            return true;
        }


        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if(await ValidateLogin())
            {

                //логін користувача

                MessageBox.Show("Успішний вхід!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);

                //відкриття головного вікна

                this.Close();
            }
        }

        
    }
}
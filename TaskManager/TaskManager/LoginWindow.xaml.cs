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
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;



namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly UserService _userService;
        private Action<bool> serverMessage;
        

        public LoginWindow(UserService userService)
        {
            InitializeComponent();
            
            _userService = userService;
            _userService.LoginRequestReceived += OnMessageReceive;
            
            //Task.Run(async() => await App.Client.ConnectAsync("127.0.0.1", 5000));
        }

        private async void OnMessageReceive(bool isReceived)
        {
            if (isReceived == true)
            {
                MessageBox.Show("Успішний вхід!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);


                var projWindow = App.ServiceProvider.GetService<ProjectsWindow>();
                projWindow.Show();
                await Dispatcher.BeginInvoke(() => this.Close());

            }
            else
            {
                MessageBox.Show("Неправельний логін або пароль", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                var user = new User
                {
                    Login = LoginTextBox.Text,
                    PasswordHash = PasswordTextBox.Text,
                };
                await _userService.LoginRequest(user);
            }
        }

        
    }
}
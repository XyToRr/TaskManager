using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
using BLL.Services;
using Domain.Models;

namespace TaskManager
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        //private readonly TaskManagerClient _taskManagerClient;
        private readonly UserService _userService;
        public RegisterWindow(UserService userService)
        {
            InitializeComponent();
            //_taskManagerClient = taskManagerClient;
            _userService = userService;

            _userService.RegisterRequestReceived += OnMessageReceived;
        }

        private void OnMessageReceived(bool isRegiter)
        {
            if (isRegiter)
            {
                MessageBox.Show("Реєстрація успішна!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                //this.Close();
            }
        }

        private async void ClearForm()
        {
            LoginTextBox.Text = string.Empty;
            PasswordTextBox.Text = string.Empty;
            PasswordRepeatTextBox.Text = string.Empty;
            FirstNameTextBox.Text = string.Empty;
            LastNameTextBox.Text = string.Empty;
        }

        private async Task<bool> ValidateRegister()
        {

            if (string.IsNullOrWhiteSpace(LoginTextBox.Text))
            {
                MessageBox.Show("Логін не може бути порожнім.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }


            if (string.IsNullOrWhiteSpace(PasswordTextBox.Text))
            {
                MessageBox.Show("Пароль не може бути порожнім.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (PasswordTextBox.Text.Length < 6)
            {
                MessageBox.Show("Пароль повинен містити щонайменше 6 символів.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }


            if (PasswordTextBox.Text != PasswordRepeatTextBox.Text)
            {
                MessageBox.Show("Паролі не співпадають.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }


            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
            {
                MessageBox.Show("Ім'я не може бути порожнім.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }


            if (string.IsNullOrWhiteSpace(LastNameTextBox.Text))
            {
                MessageBox.Show("Прізвище не може бути порожнім.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }


            
            


            return true;

        }
        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (await ValidateRegister())
            {
                var user = new User
                {
                    FirstName = FirstNameTextBox.Text,
                    LastName = LastNameTextBox.Text,
                    Login = LoginTextBox.Text,
                    PasswordHash = PasswordTextBox.Text,
                };
                await _userService.RegisterRequest(user);
            }
            else
            {
                ClearForm();
            }
        }
    }
}

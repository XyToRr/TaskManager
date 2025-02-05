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
    public partial class MainWindow : Window
    {
        private readonly UserService _userService;
        public MainWindow(UserService userService)
        {
            _userService = userService;

            _userService.RegisterRequestReceived += OnMessageRegisterReceived;
            _userService.LoginRequestReceived += OnMessageLoginReceived;
            InitializeComponent();
        }

        private void OnMessageRegisterReceived(bool isRight)
        {
            if (isRight)
            {
                return;
            }
            else
            {
                return;
            }
        }

        private void OnMessageLoginReceived(bool isRight)
        {
            if (isRight)
            {
                return;
            }
            else
            {
                return;
            }
        }
    }
}
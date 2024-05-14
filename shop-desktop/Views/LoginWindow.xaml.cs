using System;
using System.Windows;
using System.Threading.Tasks;
using shop_desktop.Services;

namespace shop_desktop.Views
{
    public partial class LoginWindow : Window
    {
        private AuthenticationService _authenticationService;
        public LoginWindow(AuthenticationService authenticationService)
        {
            InitializeComponent();
            _authenticationService = authenticationService;
        }
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordTextBox.Password;

            try
            {
                var (success, token, userId) = await _authenticationService.LoginAsync(email, password);

                if (success)
                {
                    _authenticationService.AccessToken = token;
                    Console.WriteLine($"Token: {token}, UserID: {userId}");
                    MessageBox.Show("Logowanie powiodło się!");
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Logowanie nie powiodło się. Spróbuj ponownie.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas logowania: {ex.Message}");
            }
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
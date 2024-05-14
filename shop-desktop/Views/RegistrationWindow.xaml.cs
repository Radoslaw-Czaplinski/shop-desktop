using System;
using System.Windows;
using shop_desktop.Services;

namespace shop_desktop
{
    public partial class RegistrationWindow : Window
    {
        internal ApiClient _apiClient;
        public RegistrationWindow()
        {
            InitializeComponent();
            _apiClient = new ApiClient();
        }
        internal async void Register_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text; 
            string username = UsernameTextBox.Text; 
            string password = PasswordTextBox.Password;
            bool registrationSuccess = await _apiClient.RegisterAsync(email, username, password);

            if (registrationSuccess)
            {
                MessageBox.Show("Rejestracja użytkownika powiodła się!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Rejestracja użytkownika nie powiodła się. Spróbuj ponownie.");
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
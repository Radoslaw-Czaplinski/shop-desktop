using System;
using System.Windows;

namespace shop_desktop
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        // Metoda obsługująca zdarzenie kliknięcia przycisku logowania
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            // Tutaj umieść logikę logowania
        }

        // Metoda obsługująca zdarzenie kliknięcia przycisku rejestracji
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            // Otwórz okno rejestracji
            var registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
        }      
    }
}

using System.Text;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace shop_desktop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //InitializeComponent();
        }

    // Metoda obsługująca zdarzenie kliknięcia przycisku Log in
    private void Login_Click(object sender, RoutedEventArgs e)
        {
            // Tutaj umieść logikę logowania
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        // Metoda obsługująca zdarzenie kliknięcia przycisku Register
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            // Tutaj otwórz okno rejestracji użytkownika
            var registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
        }

        // Metoda obsługująca zdarzenie kliknięcia przycisku Skip login
        private void SkipLogin_Click(object sender, RoutedEventArgs e)
        {
            // Tutaj umieść logikę pominięcia logowania
        }
    }
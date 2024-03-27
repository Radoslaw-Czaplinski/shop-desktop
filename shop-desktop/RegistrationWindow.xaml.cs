using System.Windows;

namespace shop_desktop
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        // Metoda obsługująca zdarzenie kliknięcia przycisku Register
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            // Tutaj umieść logikę rejestracji użytkownika
        }

        // Metoda obsługująca zdarzenie kliknięcia przycisku Cancel
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

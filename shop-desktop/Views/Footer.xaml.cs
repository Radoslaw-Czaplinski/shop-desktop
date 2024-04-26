using System.Windows;
using System.Windows.Controls;
using System.Windows.Input; // Dodaj tę dyrektywę

namespace shop_desktop.Views
{
    public partial class Footer : UserControl
    {
        public Footer()
        {
            InitializeComponent();
        }

        private void SendIssueButton_Click(object sender, RoutedEventArgs e)
        {
            string issueDescription = IssueTextBox.Text;

            // Tutaj możesz przetwarzać zgłoszenie problemu lub sugestię, np. wysyłać je na serwer, zapisywać w bazie danych itp.
            // W tym przykładzie pokażemy jedynie komunikat informujący, że zgłoszenie zostało wysłane.

            MessageBox.Show("Zgłoszenie zostało wysłane.", "Potwierdzenie", MessageBoxButton.OK, MessageBoxImage.Information);
            IssueTextBox.Text = ""; // Wyczyść pole tekstowe po wysłaniu zgłoszenia
        }
    }
}

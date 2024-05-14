using System.Windows;
using System.Windows.Controls;
using System.Windows.Input; 

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

            MessageBox.Show("Zgłoszenie zostało wysłane.", "Potwierdzenie", MessageBoxButton.OK, MessageBoxImage.Information);
            IssueTextBox.Text = ""; 
        }
    }
}
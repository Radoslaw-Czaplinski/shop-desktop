using System.Windows;
using shop_desktop.Services;
using shop_desktop.Views;

namespace shop_desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PostService postService;
        private bool isUserLoggedIn = true;
        private AddPostWindow addPostWindow; 

        public MainWindow()
        {
            InitializeComponent();
            postService = new PostService();
            LoadPosts();
        }

        private void LoadPosts()
        {
            // Pobieramy posty z serwisu i przypisujemy je do ListBox
            postListBox.ItemsSource = postService.GetPosts();
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

        private void AddPostButton_Click(object sender, RoutedEventArgs e)
        {
            if (isUserLoggedIn)
            {
                // Utwórz nowe okno dodawania posta
                addPostWindow = new AddPostWindow(postService);
                // Wyświetl okno
                addPostWindow.Closed += AddPostWindow_Closed;
                addPostWindow.Show();
            }
            else
            {
                MessageBox.Show("Aby dodać wpis, należy się zalogować.");
            }
        }

        private void AddPostWindow_Closed(object sender, EventArgs e)
        {
            if (addPostWindow.DialogResult.HasValue && addPostWindow.DialogResult.Value)
            {
                // Zaktualizuj wyświetlanie postów
                LoadPosts();
            }
        }
    }
}

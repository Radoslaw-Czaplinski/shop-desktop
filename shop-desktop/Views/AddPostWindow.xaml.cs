using System;
using System.Windows;
using shop_desktop.Services;
using shop_desktop.ViewModels;

namespace shop_desktop.Views
{
    public partial class AddPostWindow : Window
    {
        private readonly PostService postService;
        private readonly MainViewModel mainViewModel;

        public AddPostWindow(PostService postService, MainViewModel mainViewModel)
        {
            InitializeComponent();
            this.postService = postService;
            this.mainViewModel = mainViewModel;
            this.DataContext = mainViewModel;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string content = ContentTextBox.Text;
            string author = AuthorTextBox.Text;

            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(author))
            {
                postService.AddPost(title, content, author);
                mainViewModel.LoadPosts(); 
                Close();
            }
            else
            {
                MessageBox.Show("Proszę wypełnić wszystkie pola.");
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        this.Close(); // Zamknięcie okna
    }
    }
}

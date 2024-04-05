using System;
using System.Windows;
using shop_desktop.Services;

namespace shop_desktop.Views
{
    /// <summary>
    /// Interaction logic for AddPostWindow.xaml
    /// </summary>
    public partial class AddPostWindow : Window
    {
        private readonly PostService postService;

        public AddPostWindow(PostService postService)
        {
            InitializeComponent();
            this.postService = postService;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string content = ContentTextBox.Text;
            string author = AuthorTextBox.Text;

            postService.AddPost(title, content, author);

            DialogResult = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Close();
        }
    }
}

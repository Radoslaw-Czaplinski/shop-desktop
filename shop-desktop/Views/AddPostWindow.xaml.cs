using System;
using System.Windows;
using shop_desktop.Services;
using shop_desktop.ViewModels;
using shop_desktop.Models;

namespace shop_desktop.Views
{
    public partial class AddPostWindow : Window
    {
        private readonly PostService postService;
        private readonly MainViewModel mainViewModel;
        private readonly Post postToEdit;
        private readonly PostDetailsWindow postDetailsWindow;
        public event Action<Post> PostUpdated;

        public AddPostWindow(PostService postService, MainViewModel mainViewModel, Post postToEdit)
        {
            InitializeComponent();
            this.postService = postService;
            this.mainViewModel = mainViewModel;
            this.postToEdit = postToEdit;
            this.DataContext = postToEdit;
           
            Loaded += (sender, e) =>
            {
                if (postToEdit != null)
                {
                    TitleTextBox.Text = postToEdit.Title;
                    ContentTextBox.Text = postToEdit.Content;
                    AuthorTextBox.IsReadOnly = true;
                    AuthorTextBox.Text = postToEdit.Author;
                }
            };
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string content = ContentTextBox.Text;
            string author = AuthorTextBox.Text;
            DateTime dateAdded = DateTime.Now;

            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(content))
            {
                if (postToEdit != null)
                {
                    postToEdit.Title = title;
                    postToEdit.Content = content;
                    postService.UpdatePost(postToEdit);
                }
                else
                {
                    postService.AddPost(title, content, author, dateAdded);
                }

                mainViewModel.LoadPosts();

                Close();
            }
            else
            {
                MessageBox.Show("Proszę wypełnić tytuł i treść posta.");
            }
        }



        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}

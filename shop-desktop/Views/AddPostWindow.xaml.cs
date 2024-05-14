using System;
using System.Windows;
using shop_desktop.Services;
using shop_desktop.ViewModels;
using shop_desktop.Models;

namespace shop_desktop.Views
{
    public partial class AddPostWindow : Window
    {
        private readonly PostService _postService;
        private readonly MainViewModel _mainViewModel;
        private readonly AuthenticationService _authenticationService;
        private readonly Post _postToEdit;
        public event Action<Post> PostUpdated;
        public AddPostWindow(PostService postService, MainViewModel mainViewModel, AuthenticationService authenticationService, Post postToEdit)
        {
            InitializeComponent();
            _postService = postService;
            _mainViewModel = mainViewModel;
            _authenticationService = authenticationService;
            _postToEdit = postToEdit;
            DataContext = _postToEdit;
           
            Loaded += (sender, e) =>
            {
                if (_postToEdit != null)
                {
                    TitleTextBox.Text = _postToEdit.Title;
                    ContentTextBox.Text = _postToEdit.Content;
                    AuthorTextBox.IsReadOnly = true;
                    AuthorTextBox.Text = _postToEdit.AuthorId;
                }
            };
        }     
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string content = ContentTextBox.Text;

            if (_authenticationService != null)
            {
                string userId = _authenticationService.UserId;

                if (_postToEdit == null)
                {
                    var success = await _postService.AddPostAsync(title, content, userId);
                    if (success)
                    {
                        MessageBox.Show("Post został dodany pomyślnie.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Dodawanie posta nie powiodło się. Spróbuj ponownie.");
                    }
                }
                else
                {
                    _postToEdit.Title = title;
                    _postToEdit.Content = content;

                    var success = await _postService.UpdatePostAsync(_postToEdit);
                    if (success)
                    {
                        MessageBox.Show("Post został zaktualizowany pomyślnie.");
                        this.Close();
                        PostUpdated?.Invoke(_postToEdit);
                    }
                    else
                    {
                        MessageBox.Show("Aktualizacja posta nie powiodła się. Spróbuj ponownie.");
                    }
                }
            }
            else
            {
                MessageBox.Show("AuthenticationService nie został zainicjowany.");
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); 
        }
    }
}
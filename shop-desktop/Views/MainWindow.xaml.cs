using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using shop_desktop.ViewModels;
using shop_desktop.Services;
using shop_desktop.Models;
using shop_desktop.Views;

namespace shop_desktop
{
    public partial class MainWindow : Window
    {
        private readonly PostService _postService;
        private readonly AuthenticationService _authenticationService;
        public MainWindow()
        {
            InitializeComponent();
            var httpClient = new HttpClient();
            _authenticationService = new AuthenticationService(httpClient);
            string baseUrl = "https://bd73-82-139-13-67.ngrok-free.app/";
            _postService = new PostService(httpClient, _authenticationService, baseUrl);
            DataContext = new MainViewModel(_postService, _authenticationService);
            Loaded += MainWindow_Loaded;
        }
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadPostsAsync();
        }
        public async Task LoadPostsAsync()
        {
            if (DataContext is MainViewModel viewModel)
            {
                await viewModel.LoadPostsAsync();
            }
        }
        private async void RefreshPostsButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadPostsAsync();
        }
        private void UpdatedPostHandler(Post updatedPost)
        {
            if (DataContext is MainViewModel viewModel)
            {
                int index = viewModel.Posts.IndexOf(viewModel.Posts.FirstOrDefault(p => p.Id == updatedPost.Id));
                if (index != -1)
                {
                    viewModel.Posts[index] = updatedPost;
                }
            }
        }
        private void PostsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is Post selectedPost)
            {
                PostDetailsWindow detailsWindow = new PostDetailsWindow(selectedPost, _postService, (MainViewModel)DataContext, _authenticationService);
                detailsWindow.ShowDialog();

                if (detailsWindow.DialogResult.HasValue && detailsWindow.DialogResult.Value)
                {
                    LoadPostsAsync();
                }
            }
        }
    }
}
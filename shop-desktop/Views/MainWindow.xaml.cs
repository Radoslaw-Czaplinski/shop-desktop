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
        private PostService postService;
        public MainWindow()
        {
            InitializeComponent();
            postService = new PostService();
            DataContext = new MainViewModel(postService);
        }

        public void LoadPosts()
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.LoadPosts();
            }
        }

        private void PostsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is Post selectedPost)
            {
                PostDetailsWindow detailsWindow = new PostDetailsWindow(selectedPost, postService, (MainViewModel)DataContext);
                detailsWindow.ShowDialog();

                if (detailsWindow.DialogResult.HasValue && detailsWindow.DialogResult.Value)
                {
                    LoadPosts();
                }
            }
        }
        

    }
}

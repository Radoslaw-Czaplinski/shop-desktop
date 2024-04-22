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
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(new PostService());
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
                PostDetailsWindow detailsWindow = new PostDetailsWindow(selectedPost);
                detailsWindow.ShowDialog();
            }
        }


    }
}

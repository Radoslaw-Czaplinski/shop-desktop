using shop_desktop.Models;
using shop_desktop.Services;
using shop_desktop.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Linq; // Dodaj using System.Linq; na początku pliku

namespace shop_desktop.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Post> posts;
        public ObservableCollection<Post> Posts
        {
            get { return posts; }
            set
            {
                posts = value;
                OnPropertyChanged(nameof(Posts));
            }
        }

        private ObservableCollection<Post> filteredPosts;
        public ObservableCollection<Post> FilteredPosts
        {
            get { return filteredPosts; }
            set
            {
                filteredPosts = value;
                OnPropertyChanged(nameof(FilteredPosts));
            }
        }

        private readonly PostService postService;

        public ICommand LoginCommand { get; private set; }
        public ICommand RegisterCommand { get; private set; }
        public ICommand SkipLoginCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public ICommand AddPostCommand { get; private set; }
        public ICommand SearchCommand { get; private set; } // Dodaj pole do przechowywania komendy wyszukiwania

        // Basic constructor
        public MainViewModel(PostService postService)
        {
            this.postService = postService;
            LoadPosts();

            LoginCommand = new RelayCommand(ExecuteLogin);
            RegisterCommand = new RelayCommand(ExecuteRegister);
            SkipLoginCommand = new RelayCommand(ExecuteSkipLogin);
            RefreshCommand = new RelayCommand(ExecuteRefresh);
            AddPostCommand = new RelayCommand(ExecuteAddPost);
            SearchCommand = new RelayCommand(ExecuteSearch);
            FilteredPosts = new ObservableCollection<Post>();
        }

        private void ExecuteLogin(object parameter)
        {
            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
        }

        private void ExecuteRegister(object parameter)
        {
            var registrationWindow = new RegistrationWindow();
            registrationWindow.ShowDialog();
        }

        private void ExecuteSkipLogin(object parameter)
        {
            MessageBox.Show("Login skipped. Full access granted.");
        }

        private void ExecuteRefresh(object parameter)
        {
            LoadPosts();
        }

        private void ExecuteAddPost(object parameter)
        {
            var addPostWindow = new AddPostWindow(postService, this);
            addPostWindow.ShowDialog();
            LoadPosts(); 
        }
    
        public void LoadPosts()
        {
            Posts = postService.LoadPosts();
        }

        public void UpdatePosts(IEnumerable<Post> posts)
        {
            Posts = new ObservableCollection<Post>(posts);
        }

        private void ExecuteSearch(object parameter)
    {
        string searchTerm = parameter as string; 

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            // Jeśli fraza jest pusta, wyświetl wszystkie posty
            FilteredPosts = new ObservableCollection<Post>(Posts);
        }
        else
        {
            // Filtrowanie postów na podstawie frazy
            FilteredPosts = new ObservableCollection<Post>(Posts.Where(p => p.Title.Contains(searchTerm) || p.Content.Contains(searchTerm)));
        }
    }


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

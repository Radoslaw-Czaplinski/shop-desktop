using shop_desktop.Models;
using shop_desktop.Services;
using shop_desktop.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace shop_desktop.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly PostService postService;

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

        public ICommand LoginCommand { get; private set; }
        public ICommand RegisterCommand { get; private set; }
        public ICommand SkipLoginCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public ICommand AddPostCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public ICommand EditPostCommand { get; private set; }
        public ICommand DeletePostCommand { get; private set; }
        public ICommand ContactAuthorCommand { get; private set; }
        public ICommand SendContactMessageCommand { get; private set; }
        public ICommand SortPostsCommand { get; private set; }

        public MainViewModel(PostService postService)
        {
            this.postService = postService;

            LoginCommand = new RelayCommand(ExecuteLogin);
            RegisterCommand = new RelayCommand(ExecuteRegister);
            SkipLoginCommand = new RelayCommand(ExecuteSkipLogin);
            RefreshCommand = new RelayCommand(ExecuteRefresh);
            AddPostCommand = new RelayCommand(ExecuteAddPost);
            SearchCommand = new RelayCommand(ExecuteSearch);
            FilteredPosts = new ObservableCollection<Post>();
            EditPostCommand = new RelayCommand(EditPost);
            DeletePostCommand = new RelayCommand(DeletePost);
            ContactAuthorCommand = new RelayCommand(ExecuteContactAuthor); 
            SendContactMessageCommand = new RelayCommand(ExecuteSendContactMessage);
            SortPostsCommand = new RelayCommand(SortPosts);

            LoadPosts();
        }

        public void LoadPosts()
        {
            Posts = postService.LoadPosts();
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
            var addPostWindow = new AddPostWindow(postService, this, null); 
            addPostWindow.ShowDialog();
            LoadPosts();
        }
        private void ExecuteSearch(object parameter)
        {
            string searchTerm = parameter as string;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                FilteredPosts = new ObservableCollection<Post>(Posts);
            }
            else
            {
                FilteredPosts = new ObservableCollection<Post>(Posts.Where(p => p.Title.Contains(searchTerm) || p.Content.Contains(searchTerm)));
            }
        }

        private void EditPost(object parameter)
        {
            var postToEdit = parameter as Post;
            if (postToEdit == null) return; 

            var editPostWindow = new AddPostWindow(postService, this, postToEdit);
            editPostWindow.ShowDialog();
        }

        private void DeletePost(object parameter)
        {
            var postToDelete = parameter as Post;
            if (postToDelete == null) throw new ArgumentNullException("Post to delete cannot be null.");
            postService.DeletePost(postToDelete);
            Posts.Remove(postToDelete);
        }

        public void UpdatePostInView(Post updatedPost)
        {
            int index = Posts.IndexOf(updatedPost);
            if (index != -1)
            {
                Posts[index] = updatedPost;
                OnPropertyChanged(nameof(Posts));
            }
        }
        private void ExecuteContactAuthor(object parameter)
        {
            MessageBox.Show("Contact form opened."); 
        }

        private void ExecuteSendContactMessage(object parameter)
        {
            MessageBox.Show("Message sent successfully!"); 
        }
        private void SortPosts(object parameter)
        {
            var sortOption = parameter as string;
            switch (sortOption)
            {
                case "Tytuł":
                    Posts = new ObservableCollection<Post>(Posts.OrderBy(p => p.Title));
                    break;
                case "Autor":
                    Posts = new ObservableCollection<Post>(Posts.OrderBy(p => p.Author));
                    break;
                case "Data dodania (od najnowszego)":
                    Posts = new ObservableCollection<Post>(Posts.OrderByDescending(p => p.DateAdded));
                    break;
                case "Data dodania (od najstarszego)":
                    Posts = new ObservableCollection<Post>(Posts.OrderBy(p => p.DateAdded));
                    break;
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

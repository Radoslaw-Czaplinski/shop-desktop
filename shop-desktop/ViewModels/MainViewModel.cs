using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Newtonsoft.Json;
using shop_desktop.Models;
using shop_desktop.Services;
using shop_desktop.Views;

namespace shop_desktop.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly PostService _postService;
        private readonly AuthenticationService _authenticationService;
        private ObservableCollection<Post> _posts;
        private ObservableCollection<Post> _filteredPosts;
        private Dictionary<int, DateTimeOffset> _postsDates;
        public event Action<Post> PostUpdated;
        private string _currentSortOption;
        private string _currentUserId;
        private string _searchTerm;

        public ObservableCollection<Post> Posts
        {
            get { return _posts; }
            set
            {
                if (_posts != value)
                {
                    _posts = value;
                    OnPropertyChanged(nameof(Posts));
                    ApplyFilterAndSort();
                }
            }
        }
        public ObservableCollection<Post> FilteredPosts
        {
            get { return _filteredPosts; }
            set
            {
                if (_filteredPosts != value)
                {
                    _filteredPosts = value;
                    OnPropertyChanged(nameof(FilteredPosts));
                }
            }
        }
        public string CurrentUserId
        {
            get { return _currentUserId; }
            set
            {
                if (_currentUserId != value)
                {
                    _currentUserId = value;
                    OnPropertyChanged(nameof(CurrentUserId));
                    LoadPostsAsync();
                }
            }
        }
        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                if (_searchTerm != value)
                {
                    _searchTerm = value;
                    OnPropertyChanged(nameof(SearchTerm));
                    ApplyFilterAndSort();
                }
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

        public MainViewModel(PostService postService, AuthenticationService authenticationService)
        {
            _postService = postService;
            _authenticationService = authenticationService;

            LoginCommand = new RelayCommand(ExecuteLogin);
            RegisterCommand = new RelayCommand(ExecuteRegister);
            SkipLoginCommand = new RelayCommand(ExecuteSkipLogin);
            RefreshCommand = new RelayCommand(ExecuteRefresh);
            AddPostCommand = new RelayCommand(ExecuteAddPost);
            SearchCommand = new RelayCommand(ExecuteSearch);
            EditPostCommand = new RelayCommand(EditPost);
            DeletePostCommand = new RelayCommand(DeletePost);
            ContactAuthorCommand = new RelayCommand(ExecuteContactAuthor);
            SendContactMessageCommand = new RelayCommand(ExecuteSendContactMessage);
            SortPostsCommand = new RelayCommand(SortPosts);

            Posts = new ObservableCollection<Post>();
            FilteredPosts = new ObservableCollection<Post>();
            _postsDates = LocalDataStore.LoadPostsDates();
            LoadPostsAsync();
        }
        public void UpdateCurrentUserId()
        {
            CurrentUserId = _authenticationService.UserId;
            OnPropertyChanged(nameof(CurrentUserId));
        }
        public async Task LoadPostsAsync()
        {
            var posts = await _postService.LoadPostsAsync();
            foreach (var post in posts)
            {
                if (_postsDates.ContainsKey(post.Id))
                {
                    post.DateAdded = _postsDates[post.Id];
                }
                else
                {
                    post.DateAdded = DateTimeOffset.Now;
                    _postsDates[post.Id] = post.DateAdded;
                }
            }
            LocalDataStore.SavePostsDates(_postsDates);
            Posts = new ObservableCollection<Post>(SortPosts(posts));
            ApplyFilterAndSort();
        }
        private async void ExecuteLogin(object parameter)
        {
            var loginWindow = new LoginWindow(_authenticationService);
            loginWindow.ShowDialog();
            var result = loginWindow.DialogResult;
            if (result.HasValue && result.Value)
            {
                var token = _authenticationService.AccessToken;
                if (!string.IsNullOrEmpty(token))
                {
                    _postService.SetAuthorizationHeader(token);
                    UpdateCurrentUserId();
                    MessageBox.Show("Login successful!");
                }
                else
                {
                    MessageBox.Show("Access token is empty.");
                }
            }
            else
            {
                MessageBox.Show("Login process was not successful.");
            }
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
            LoadPostsAsync();
        }
        private void ExecuteAddPost(object parameter)
        {
            Console.WriteLine($"Current user ID at add post: {CurrentUserId}");
            if (!string.IsNullOrEmpty(CurrentUserId))  
            {
                var addPostWindow = new AddPostWindow(_postService, this, _authenticationService, null);
                addPostWindow.ShowDialog();
                LoadPostsAsync(); 
            }
            else
            {
                MessageBox.Show("User ID is not set. Please log in.");
            }
        }
        private void ApplyFilterAndSort()
        {
            var filteredPosts = string.IsNullOrWhiteSpace(SearchTerm)
                ? Posts
                : new ObservableCollection<Post>(Posts.Where(p => p.Title.Contains(SearchTerm) || p.Content.Contains(SearchTerm)));
            FilteredPosts = new ObservableCollection<Post>(SortPosts(filteredPosts));
        }
        private void ExecuteSearch(object parameter)
        {
            ApplyFilterAndSort();
        }
        private async void EditPost(object parameter)
        {
            var postToEdit = parameter as Post;
            if (postToEdit == null) return;
            var editPostWindow = new AddPostWindow(_postService, this, _authenticationService, postToEdit);
            editPostWindow.ShowDialog();
            var success = await _postService.UpdatePostAsync(postToEdit);
            if (success)
            {
                MessageBox.Show("Post updated successfully!");
            }
            else
            {
                MessageBox.Show("Failed to update post. Please try again.");
            }
        }
        private async void DeletePost(object parameter)
        {
            var postToDelete = parameter as Post;
            if (postToDelete == null) return;
            var confirmation = MessageBox.Show("Are you sure you want to delete this post?", "Confirm Deletion", MessageBoxButton.YesNo);
            if (confirmation == MessageBoxResult.Yes)
            {
                var success = await _postService.DeletePostAsync(postToDelete.Id);
                if (success)
                {
                    MessageBox.Show("Post deleted successfully!");
                    Posts.Remove(postToDelete);
                }
                else
                {
                    MessageBox.Show("Failed to delete post. Please try again.");
                }
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
        private IEnumerable<Post> SortPosts(IEnumerable<Post> posts)
        {
            switch (_currentSortOption)
            {
                case "Tytuł":
                    return posts.OrderBy(p => p.Title);
                case "Data dodania (od najnowszego)":
                    return posts.OrderByDescending(p => p.DateAdded);
                case "Data dodania (od najstarszego)":
                    return posts.OrderBy(p => p.DateAdded);
                default:
                    return posts;
            }
        }
        private void SortPosts(object parameter)
        {
            string sortOption = parameter as string;
            Console.WriteLine($"Sort option: {sortOption}");
            if (sortOption == null) return;

            _currentSortOption = sortOption;

            var sortedPosts = SortPosts(Posts);
            foreach (var post in sortedPosts)
            {
                Console.WriteLine($"Post: {post.Title}, DateAdded: {post.DateAdded}");
            }
            Posts = new ObservableCollection<Post>(sortedPosts);
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
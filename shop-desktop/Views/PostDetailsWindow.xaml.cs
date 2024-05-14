using System;
using System.Threading.Tasks;
using System.Windows;
using shop_desktop.Models;
using shop_desktop.Services;
using shop_desktop.ViewModels;

namespace shop_desktop.Views
{
    public partial class PostDetailsWindow : Window
    {
        private Post post;
        private readonly PostService _postService;
        private readonly MainViewModel _mainViewModel;
        private readonly AuthenticationService _authenticationService;
        public event Action<Post> PostUpdated;
        public PostDetailsWindow(Post post, PostService postService, MainViewModel mainViewModel, AuthenticationService authenticationService)
        {
            InitializeComponent();
            this.post = post;
            _postService = postService;
            _mainViewModel = mainViewModel;
            _authenticationService = authenticationService;
            DataContext = this.post;
            commentsListBox.ItemsSource = post.Comments;
            ratingsListBox.ItemsSource = post.Ratings;

            IncrementViewsCount();
            LoadCommentsForPostAsync(post.Id);
            LoadRatingsForPostAsync(post.Id);
            LoadViewsCount(post.Id);
        }
        public async Task LoadCommentsForPostAsync(int postId)
        {
            try
            {
                var comments = await _postService.LoadCommentsAsync(postId);
                post.Comments.Clear();
                foreach (var comment in comments)
                {
                    post.Comments.Add(comment);
                }
                commentsListBox.Items.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load comments: {ex.Message}");
            }
        }
        public async Task LoadRatingsForPostAsync(int postId)
        {
            try
            {
                var ratings = await _postService.LoadRatingsAsync(postId);
                post.Ratings.Clear();
                foreach (var rating in ratings)
                {
                    post.Ratings.Add(rating);
                }
                ratingsListBox.Items.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load ratings: {ex.Message}");
            }
        }
        private async void LoadViewsCount(int postId)
        {
            try
            {
                var postDetails = await _postService.GetPostDetailsAsync(postId);
                ViewsCountTextBlock.Text = $"Liczba wyświetleń: {postDetails.Views}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load views count: {ex.Message}");
            }
        }
        private async void AddCommentButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string commentContent = commentTextBox.Text;
                string userId = _authenticationService.UserId;
                var commentData = new { content = commentContent, author_id = userId };
                bool success = await _postService.AddCommentAsync(post.Id, commentData);

                if (success)
                {
                    UpdateCommentsListBox();
                    commentTextBox.Clear();
                    MessageBox.Show("Komentarz został dodany.", "Potwierdzenie", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Ukryj okno komunikatu błędu
                    //MessageBox.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas dodawania komentarza: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void UpdateCommentsListBox()
        {
            commentsListBox.ItemsSource = null;
            commentsListBox.ItemsSource = post.Comments;
            commentsListBox.Items.Refresh();
        }
        private async void AddRatingButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ratingTextBox.Text, out int ratingValue))
            {
                if (ratingValue >= 1 && ratingValue <= 5)
                {
                    try
                    {
                        await _postService.AddRatingAsync(post.Id, ratingValue);
                        MessageBox.Show("Ocena została dodana.", "Potwierdzenie", MessageBoxButton.OK, MessageBoxImage.Information);
                        UpdateRatingsListBox();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Wystąpił błąd podczas dodawania oceny: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Ocena musi być liczbą całkowitą z zakresu od 1 do 5.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Wprowadź poprawną liczbę całkowitą z zakresu od 1 do 5.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void UpdateRatingsListBox()
        {
            ratingsListBox.ItemsSource = null;
            ratingsListBox.ItemsSource = post.Ratings;
            ratingsListBox.Items.Refresh();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new AddPostWindow(_postService, _mainViewModel, _authenticationService, post);
            editWindow.PostUpdated += UpdatedPostHandler;
            editWindow.ShowDialog();
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz usunąć ten post?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                _mainViewModel.DeletePostCommand.Execute(post);
                this.Close();
            }
        }
        private void UpdatedPostHandler(Post updatedPost)
        {
            post.Title = updatedPost.Title;
            post.Content = updatedPost.Content;
            DataContext = null;
            DataContext = post;
            PostUpdated?.Invoke(post);
        }
        private void ContactAuthor_Click(object sender, RoutedEventArgs e)
        {
            ContactForm.Visibility = Visibility.Visible;
        }
        private async void SendContactForm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string subject = subjectTextBox.Text;
                string message = messageTextBox.Text;
                await _postService.PostAsync(subject, message);

                MessageBox.Show("Wiadomość została wysłana.", "Komunikat", MessageBoxButton.OK, MessageBoxImage.Information);
                ContactForm.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas wysyłania wiadomości: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void IncrementViewsCount()
        {
            post.ViewsCount++;
            ViewsCountTextBlock.Text = $"Liczba wyświetleń: {post.ViewsCount}";
        }
    }
}
using System;
using System.Windows;
using shop_desktop.Models;
using shop_desktop.Services;
using shop_desktop.ViewModels;
using System.Windows.Controls;

namespace shop_desktop.Views
{
    public partial class PostDetailsWindow : Window
    {
        private Post post;
        private PostService postService;
        private MainViewModel mainViewModel;

        public PostDetailsWindow(Post post, PostService postService, MainViewModel mainViewModel)
        {
            InitializeComponent();
            this.post = post;
            this.postService = postService;
            this.mainViewModel = mainViewModel;
            DataContext = this.post;
            commentsListBox.ItemsSource = post.Comments;
            ratingsListBox.ItemsSource = post.Ratings;

            IncrementViewsCount();
        }

        private void AddCommentButton_Click(object sender, RoutedEventArgs e)
        {
            string commentContent = commentTextBox.Text;
            string author = "User"; // Możesz ustawić autora na podstawie zalogowanego użytkownika
            DateTime date = DateTime.Now;

            Comment newComment = new Comment { Author = author, Date = date, Content = commentContent };
            post.Comments.Add(newComment);
            commentTextBox.Clear(); 
            MessageBox.Show("Komentarz został dodany.", "Potwierdzenie", MessageBoxButton.OK, MessageBoxImage.Information);
            commentsListBox.Items.Refresh();
        }

        private void AddRatingButton_Click(object sender, RoutedEventArgs e)
        {
        if (int.TryParse(ratingTextBox.Text, out int ratingValue))
        {
            if (ratingValue >= 1 && ratingValue <= 5)
            {
                string author = "User"; // Tutaj możemy pobrać nazwę użytkownika zalogowanego użytkownika
                DateTime date = DateTime.Now;

                Rating newRating = new Rating { Author = author, Date = date, Value = ratingValue };
                post.Ratings.Add(newRating);
                MessageBox.Show("Ocena została dodana.", "Potwierdzenie", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateRatingsListBox();
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
            ratingsListBox.ItemsSource = post.Ratings; // Przypisz listę ocen
            ratingsListBox.Items.Refresh();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new AddPostWindow(postService, mainViewModel, post);
            editWindow.PostUpdated += UpdatedPostHandler; // Zarejestruj się na zdarzenie zaktualizowanego posta
            editWindow.ShowDialog();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz usunąć ten post?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                mainViewModel.DeletePostCommand.Execute(post);
                this.Close(); // Zamknij okno szczegółów po usunięciu postu
            }
        }
        private void UpdatedPostHandler(Post updatedPost)
        {
            post = updatedPost;
            DataContext = post;
        }
        private void ContactAuthor_Click(object sender, RoutedEventArgs e)
        {
            // Pokazanie formularza kontaktowego
            ContactForm.Visibility = Visibility.Visible;
        }

        private void SendContactForm_Click(object sender, RoutedEventArgs e)
        {
            // Wysłanie formularza (tutaj można dodać walidację i logikę wysyłki)
            MessageBox.Show("Wiadomość została wysłana.", "Komunikat", MessageBoxButton.OK, MessageBoxImage.Information);
            ContactForm.Visibility = Visibility.Collapsed;  // Ukrycie formularza po wysłaniu
        }
        private void IncrementViewsCount()
        {
            post.ViewsCount++; // Inkrementuj liczbę wyświetleń
            ViewsCountTextBlock.Text = $"Liczba wyświetleń: {post.ViewsCount}";
        }

    }
}

using System;
using System.Windows;
using shop_desktop.Models;

namespace shop_desktop.Views
{
    public partial class PostDetailsWindow : Window
    {
        private Post post;

        public PostDetailsWindow(Post post)
        {
            InitializeComponent();
            this.post = post;
            DataContext = this.post;
            commentsListBox.ItemsSource = post.Comments;
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
        ratingsListBox.ItemsSource = null; // Wyczyść poprzednią zawartość
        ratingsListBox.ItemsSource = post.Ratings; // Przypisz listę ocen
    }


    }
}

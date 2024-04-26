using System.Collections.ObjectModel;
using System.ComponentModel;
using shop_desktop.Models;

namespace shop_desktop.Services
{
    public class PostService : INotifyPropertyChanged
    {
        private ObservableCollection<Post> posts;

        public PostService()
        {
            posts = new ObservableCollection<Post>();
            // Example posts
            AddPost("Sample Title 1", "Sample Content 1", "Author 1", DateTime.Now);
            AddPost("Sample Title 2", "Sample Content 2", "Author 2", DateTime.Now.AddMinutes(-10));
        }

        public ObservableCollection<Post> LoadPosts()
        {
            return new ObservableCollection<Post>(posts.OrderByDescending(p => p.DateAdded));
        }

        public void AddPost(string title, string content, string author, DateTime dateAdded, int? postId = null)
        {
            if (postId.HasValue)
            {
                var existingPost = posts.FirstOrDefault(p => p.Id == postId);
                if (existingPost != null)
                {
                    existingPost.Title = title;
                    existingPost.Content = content;
                    existingPost.Author = author;
                    existingPost.DateAdded = dateAdded;
                }
                else
                {
                    throw new ArgumentException("Post with the specified ID not found.");
                }
            }
            else
            {
                var newPost = new Post { Title = title, Content = content, Author = author, DateAdded = dateAdded };
                posts.Add(newPost);
            }
        }

        public void UpdatePost(Post postToUpdate)
        {
            // Znajdź i zaktualizuj post w kolekcji
            var existingPost = posts.FirstOrDefault(p => p.Title == postToUpdate.Title); // Przykład identyfikacji; lepiej użyć unikalnego identyfikatora, jeśli dostępny
            if (existingPost != null)
            {
                existingPost.Title = postToUpdate.Title;
                existingPost.Content = postToUpdate.Content;
                existingPost.Author = postToUpdate.Author;
                OnPropertyChanged("Posts");
            }
        }

        public void DeletePost(Post postToDelete)
        {
            // Usuń post z kolekcji
            var existingPost = posts.FirstOrDefault(p => p.Title == postToDelete.Title); // Podobnie, użyj unikalnego identyfikatora, jeśli dostępny
            if (existingPost != null)
            {
                posts.Remove(existingPost);
                OnPropertyChanged("Posts");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

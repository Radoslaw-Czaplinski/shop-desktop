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
            AddPost("Sample Title 1", "Sample Content 1", "Author 1");
            AddPost("Sample Title 2", "Sample Content 2", "Author 2");
        }

        public ObservableCollection<Post> LoadPosts()
        {
            return posts;
        }

        public void AddPost(string title, string content, string author)
        {
            var newPost = new Post { Title = title, Content = content, Author = author };
            posts.Add(newPost);
            OnPropertyChanged("Posts");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System.Collections.Generic;
using shop_desktop.Models;

namespace shop_desktop.Services
{
    public class PostService
    {
        private List<Post> posts;

        public PostService()
        {
            // Inicjalizacja listy postów
            posts = new List<Post>();
        }

        // Metoda dodająca nowy post do listy
        public void AddPost(string title, string content, string author)
        {
            Post newPost = new Post { Title = title, Content = content, Author = author };
            posts.Add(newPost);
        }

        // Metoda pobierająca wszystkie posty
        public List<Post> GetPosts()
        {
            // Zwrócenie listy postów
            return posts;
        }
    }
}

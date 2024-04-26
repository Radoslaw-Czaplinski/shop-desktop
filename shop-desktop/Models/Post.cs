using System.Collections.Generic;

namespace shop_desktop.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime DateAdded {get; set; }
        public int ViewsCount { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
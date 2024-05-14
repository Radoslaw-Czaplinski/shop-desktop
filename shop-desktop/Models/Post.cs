using System.Collections.Generic;

namespace shop_desktop.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorId { get; set; }
        public DateTimeOffset DateAdded { get; set; }
        public string UserId { get; set; }
        public int ViewsCount { get; set; }
        public int Views { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
using System;

namespace shop_desktop.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public string AuthorId { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
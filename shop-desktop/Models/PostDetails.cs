using System;

namespace shop_desktop.Models
{
    public class PostDetails
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorId { get; set; }
        public DateTime DateAdded { get; set; }
        public int Views { get; set; }
    }
}
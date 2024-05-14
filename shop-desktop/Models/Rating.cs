using System;

namespace shop_desktop.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public int UserId { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
    }
}
using NUnit.Framework;
using shop_desktop.Models;
using shop_desktop.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Tests
{
    public class PostServiceAddPostTests
    {
        [Test]
        public void AddPost_Adds_New_Post_To_Database()
        {
            var postService = new PostService();
            string title = "New Post Title";
            string content = "New Post Content";
            string author = "New Post Author";
            DateTime dateAdded = DateTime.Now;

            postService.AddPost(title, content, author, dateAdded);

            ObservableCollection<Post> posts = postService.LoadPosts();

            Assert.IsTrue(posts.Any(p => p.Title == title && p.Content == content && p.Author == author && p.DateAdded == dateAdded));
        }
    }
}

using NUnit.Framework;
using shop_desktop.Models; 
using shop_desktop.Services; 
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Tests
{
    public class PostServiceLoadPostsTests
    {
        [Test]
        public void LoadPosts_Returns_Valid_Post_List()
        {
            ObservableCollection<Post> expectedPosts = new ObservableCollection<Post>
            {
                new Post { Title = "Sample Title 1", Content = "Sample Content 1", Author = "Author 1", DateAdded = DateTime.Now },
                new Post { Title = "Sample Title 2", Content = "Sample Content 2", Author = "Author 2", DateAdded = DateTime.Now.AddMinutes(-10) }
            };

            var postService = new PostService();

            ObservableCollection<Post> actualPosts = postService.LoadPosts();

            Assert.AreEqual(expectedPosts.Count, actualPosts.Count);

            for (int i = 0; i < expectedPosts.Count; i++)
            {
                Assert.AreEqual(expectedPosts[i].Title, actualPosts[i].Title);
                Assert.AreEqual(expectedPosts[i].Content, actualPosts[i].Content);
                Assert.AreEqual(expectedPosts[i].Author, actualPosts[i].Author);
                Assert.IsTrue((expectedPosts[i].DateAdded - actualPosts[i].DateAdded) < TimeSpan.FromMilliseconds(1));
            }
        }
    }
}

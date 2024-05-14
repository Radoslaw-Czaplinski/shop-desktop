using NUnit.Framework;
using shop_desktop.Models;

namespace shop_desktop.Tests
{
    [TestFixture]
    public class PostTests
    {
        [Test]
        public void Post_PropertiesSetCorrectly()
        {
            var post = new Post
            {
                Id = 1,
                Title = "Test Title",
                Content = "Test Content",
                AuthorId = "123"
            };

            Assert.AreEqual(1, post.Id);
            Assert.AreEqual("Test Title", post.Title);
            Assert.AreEqual("Test Content", post.Content);
            Assert.AreEqual("123", post.AuthorId);
        }
    }
}
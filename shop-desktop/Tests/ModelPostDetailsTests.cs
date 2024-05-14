using NUnit.Framework;
using shop_desktop.Models;

namespace shop_desktop.Tests
{
    [TestFixture]
    public class PostDetailsTests
    {
        [Test]
        public void PostDetails_PropertiesSetCorrectly()
        {
            var postDetails = new PostDetails
            {
                Id = 1,
                Title = "Test Title",
                Content = "Test Content",
                AuthorId = "123",
                DateAdded = DateTime.Now,
                Views = 10
            };

            Assert.AreEqual(1, postDetails.Id);
            Assert.AreEqual("Test Title", postDetails.Title);
            Assert.AreEqual("Test Content", postDetails.Content);
            Assert.AreEqual("123", postDetails.AuthorId);
            Assert.AreEqual(DateTime.Now.Date, postDetails.DateAdded.Date);
            Assert.AreEqual(10, postDetails.Views);
        }
    }
}
using NUnit.Framework;
using shop_desktop.Models;

namespace shop_desktop.Tests
{
    [TestFixture]
    public class CommentTests
    {
        [Test]
        public void Comment_PropertiesSetCorrectly()
        {
            var comment = new Comment
            {
                Id = 1,
                Content = "Test content",
                AuthorId = "123"
            };

            Assert.AreEqual(1, comment.Id);
            Assert.AreEqual("Test content", comment.Content);
            Assert.AreEqual("123", comment.AuthorId);
        }
    }
}

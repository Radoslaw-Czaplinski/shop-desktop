using NUnit.Framework;
using shop_desktop.Models;

namespace Tests
{
    public class PostPropertyTests
    {
        [Test]
        public void Post_Title_Can_Be_Set_And_Get_Correctly()
        {
            string expectedTitle = "Sample Title";
            var post = new Post();

            post.Title = expectedTitle;
            string actualTitle = post.Title;

            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [Test]
        public void Post_Author_Can_Be_Set_And_Get_Correctly()
        {
            string expectedAuthor = "Sample Author";
            var post = new Post();

            post.Author = expectedAuthor;
            string actualAuthor = post.Author;

            Assert.AreEqual(expectedAuthor, actualAuthor);
        }

        [Test]
        public void Post_Content_Can_Be_Set_And_Get_Correctly()
        {
            string expectedContent = "Sample Content";
            var post = new Post();

            post.Content = expectedContent;
            string actualContent = post.Content;

            Assert.AreEqual(expectedContent, actualContent);
        }

        [Test]
        public void Post_DateAdded_Can_Be_Set_And_Get_Correctly()
        {
            var expectedDate = DateTime.Now;
            var post = new Post();

            post.DateAdded = expectedDate;
            var actualDate = post.DateAdded;

            Assert.AreEqual(expectedDate, actualDate);
        }

    }
}

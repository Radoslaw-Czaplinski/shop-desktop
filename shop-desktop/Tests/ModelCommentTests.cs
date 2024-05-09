using NUnit.Framework;
using shop_desktop.Models;

namespace Tests
{
    public class CommentPropertyTests
    {
        [Test]
        public void Comment_Author_Can_Be_Set_And_Get_Correctly()
        {
            string expectedAuthor = "Sample Author";
            var comment = new Comment();

            comment.Author = expectedAuthor;
            string actualAuthor = comment.Author;

            Assert.AreEqual(expectedAuthor, actualAuthor);
        }

        [Test]
        public void Comment_Date_Can_Be_Set_And_Get_Correctly()
        {
            var expectedDate = DateTime.Now;
            var comment = new Comment();

            comment.Date = expectedDate;
            var actualDate = comment.Date;

            Assert.AreEqual(expectedDate, actualDate);
        }

        [Test]
        public void Comment_Content_Can_Be_Set_And_Get_Correctly()
        {
            string expectedContent = "Sample Content";
            var comment = new Comment();

            comment.Content = expectedContent;
            string actualContent = comment.Content;

            Assert.AreEqual(expectedContent, actualContent);
        }
    }
}

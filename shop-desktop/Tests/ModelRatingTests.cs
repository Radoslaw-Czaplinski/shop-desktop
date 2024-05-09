using NUnit.Framework;
using shop_desktop.Models;

namespace Tests
{
    public class RatingPropertyTests
    {
        [Test]
        public void Rating_Author_Can_Be_Set_And_Get_Correctly()
        {
            string expectedAuthor = "Sample Author";
            var rating = new Rating();

            rating.Author = expectedAuthor;
            string actualAuthor = rating.Author;

            Assert.AreEqual(expectedAuthor, actualAuthor);
        }

        [Test]
        public void Rating_Date_Can_Be_Set_And_Get_Correctly()
        {
            var expectedDate = DateTime.Now;
            var rating = new Rating();

            rating.Date = expectedDate;
            var actualDate = rating.Date;

            Assert.AreEqual(expectedDate, actualDate);
        }

        [Test]
        public void Rating_Value_Can_Be_Set_And_Get_Correctly()
        {
            int expectedValue = 5;
            var rating = new Rating();

            rating.Value = expectedValue;
            int actualValue = rating.Value;

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}

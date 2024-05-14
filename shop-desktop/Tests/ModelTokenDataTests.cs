using NUnit.Framework;
using shop_desktop.Models;

namespace shop_desktop.Tests
{
    [TestFixture]
    public class TokenDataTests
    {
        [Test]
        public void TokenData_PropertiesSetCorrectly()
        {
            var tokenData = new TokenData
            {
                access_token = "sample_token",
                user_id = "user123"
            };

            Assert.AreEqual("sample_token", tokenData.access_token);
            Assert.AreEqual("user123", tokenData.user_id);
        }
    }
}
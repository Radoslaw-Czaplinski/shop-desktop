using NUnit.Framework;
using shop_desktop.Views;
using System.Windows;

namespace Tests
{
    [TestFixture]
    [Apartment(System.Threading.ApartmentState.STA)]
    public class FooterTests
    {
        private Footer _footer;

        [SetUp]
        public void Setup()
        {
            _footer = new Footer();
        }

        [Test]
        public void Footer_Constructor_ShouldInitializeComponent()
        {
            var footer = new Footer();

            Assert.IsNotNull(footer);
        }
    }
}

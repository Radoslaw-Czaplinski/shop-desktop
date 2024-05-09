using NUnit.Framework;
using shop_desktop.Models;
using shop_desktop.Services;
using shop_desktop.ViewModels;

namespace Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public void Test_LoadPostsCommand()
        {
            // Arrange
            var postService = new PostService(); // Tworzymy instancję PostService
            var viewModel = new MainViewModel(postService); // Przekazujemy PostService do konstruktora MainViewModel

            // Act
            viewModel.RefreshCommand.Execute(null);

            // Assert
            Assert.IsNotNull(viewModel.Posts);
            Assert.Greater(viewModel.Posts.Count, 0);
        }
    }
}

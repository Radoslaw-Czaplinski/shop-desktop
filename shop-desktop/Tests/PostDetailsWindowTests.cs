using NUnit.Framework;
using shop_desktop.Views;
using shop_desktop.Services;
using shop_desktop.Models;
using shop_desktop.ViewModels;
using Moq;
using System.Net.Http;

namespace Tests
{
    [TestFixture]
    [Apartment(System.Threading.ApartmentState.STA)]
    public class PostDetailsWindowTests
    {
        private Mock<PostService> _postServiceMock;
        private Mock<AuthenticationService> _authenticationServiceMock;
        private Mock<MainViewModel> _mainViewModelMock;
        private Post _post;
        private PostDetailsWindow _postDetailsWindow;

        [SetUp]
        public void Setup()
        {
            var httpClient = new HttpClient();
            _authenticationServiceMock = new Mock<AuthenticationService>(httpClient);
            _postServiceMock = new Mock<PostService>(httpClient, _authenticationServiceMock.Object, "http://example.com");

            _mainViewModelMock = new Mock<MainViewModel>(_postServiceMock.Object, _authenticationServiceMock.Object);
            _post = new Post { Id = 1, Title = "Test Post", Content = "This is a test post." };
            _postDetailsWindow = new PostDetailsWindow(_post, _postServiceMock.Object, _mainViewModelMock.Object, _authenticationServiceMock.Object);
        }

        [Test]
        public void PostDetailsWindow_CanBeCreated()
        {
            Assert.IsNotNull(_postDetailsWindow);
        }
    }
}

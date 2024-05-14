using NUnit.Framework;
using shop_desktop.Views;
using shop_desktop.Services;
using shop_desktop.ViewModels;
using shop_desktop.Models;
using Moq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;

namespace Tests
{
    [TestFixture]
    [Apartment(System.Threading.ApartmentState.STA)]
    public class AddPostWindowTests
    {
        private AddPostWindow _addPostWindow;
        private Mock<PostService> _postServiceMock;
        private Mock<MainViewModel> _mainViewModelMock;
        private Mock<AuthenticationService> _authServiceMock;
        private Post _post;

        [SetUp]
        public void Setup()
        {
            var httpClient = new HttpClient();
            var baseUrl = "http://example.com";

            _authServiceMock = new Mock<AuthenticationService>(httpClient);
            _postServiceMock = new Mock<PostService>(httpClient, _authServiceMock.Object, baseUrl);
            _mainViewModelMock = new Mock<MainViewModel>(_postServiceMock.Object, _authServiceMock.Object);
            _post = new Post { Id = 1, Title = "Test Post", Content = "This is a test post." };
            _addPostWindow = new AddPostWindow(_postServiceMock.Object, _mainViewModelMock.Object, _authServiceMock.Object, _post);
        }
        [Test]
        public void AddPostWindow_Constructor_ShouldInitializeComponent()
        {
            var window = new AddPostWindow(_postServiceMock.Object, _mainViewModelMock.Object, _authServiceMock.Object, _post);

            Assert.IsNotNull(window);
        }
    }
}
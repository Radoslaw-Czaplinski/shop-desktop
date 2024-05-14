using NUnit.Framework;
using shop_desktop.Services;
using shop_desktop.Models;
using Moq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Moq.Protected;
using System.Threading;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    public class PostServiceTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private PostService _postService;
        private Mock<AuthenticationService> _authenticationServiceMock;
        private const string BaseUrl = "http://127.0.0.1:5000";

        [SetUp]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _authenticationServiceMock = new Mock<AuthenticationService>(httpClient);
            _postService = new PostService(httpClient, _authenticationServiceMock.Object, BaseUrl);
        }

        [Test]
        public async Task LoadPostsAsync_ReturnsPosts()
        {
            var posts = new ObservableCollection<Post>
            {
                new Post { Id = 1, Title = "Post 1", Content = "Content 1" },
                new Post { Id = 2, Title = "Post 2", Content = "Content 2" }
            };
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(posts))
                });

            var result = await _postService.LoadPostsAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(posts.Count, result.Count);
        }

        [Test]
        public async Task AddPostAsync_ValidPost_ReturnsTrue()
        {
            var post = new Post { Title = "New Post", Content = "New Content", AuthorId = "1" };
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Created
                });

            var result = await _postService.AddPostAsync(post.Title, post.Content, post.AuthorId);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdatePostAsync_ValidPost_ReturnsTrue()
        {
            var post = new Post { Id = 1, Title = "Updated Post", Content = "Updated Content", AuthorId = "1" };
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent
                });

            var result = await _postService.UpdatePostAsync(post);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeletePostAsync_ValidId_ReturnsTrue()
        {
            var postId = 1;
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent
                });

            var result = await _postService.DeletePostAsync(postId);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddRatingAsync_ValidData_DoesNotThrow()
        {
            var postId = 1;
            var ratingValue = 5;
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            Assert.DoesNotThrowAsync(async () => await _postService.AddRatingAsync(postId, ratingValue));
        }

        [Test]
        public async Task AddCommentAsync_ValidComment_ReturnsTrue()
        {
            var postId = 1;
            var commentData = new { Content = "New Comment" };
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Created
                });

            var result = await _postService.AddCommentAsync(postId, commentData);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task LoadCommentsAsync_ReturnsComments()
        {
            var comments = new List<Comment>
            {
                new Comment { Id = 1, Content = "Comment 1" },
                new Comment { Id = 2, Content = "Comment 2" }
            };
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(comments))
                });

            var result = await _postService.LoadCommentsAsync(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(comments.Count, result.Count);
        }

        [Test]
        public async Task GetPostDetailsAsync_ValidId_ReturnsPost()
        {
            var postId = 1;
            var post = new Post { Id = postId, Title = "Post Details", Content = "Detailed Content" };
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(post))
                });

            var result = await _postService.GetPostDetailsAsync(postId);
            Assert.IsNotNull(result);
            Assert.AreEqual(postId, result.Id);
            Assert.AreEqual(post.Title, result.Title);
            Assert.AreEqual(post.Content, result.Content);
        }
    }
}

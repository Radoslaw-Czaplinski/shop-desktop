using NUnit.Framework;
using Moq;
using Moq.Protected;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using shop_desktop.Services;
using Newtonsoft.Json;
using System;

namespace shop_desktop.Tests
{
    public class ApiClientTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private ApiClient _apiClient;
        [SetUp]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("http://127.0.0.1:5000")
            };
            _apiClient = (ApiClient)Activator.CreateInstance(typeof(ApiClient), nonPublic: true);
            typeof(ApiClient).GetField("_httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(_apiClient, httpClient);
        }

        [TearDown]
        public void TearDown()
        {
            var httpClient = (HttpClient)typeof(ApiClient).GetField("_httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(_apiClient);
            httpClient.Dispose();
        }

        [Test]
        public async Task RegisterAsync_SuccessfulRegistration_ReturnsTrue()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri == new Uri("http://127.0.0.1:5000/auth/register")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var result = await _apiClient.RegisterAsync("test@example.com", "testuser", "password123");
            Assert.IsTrue(result);
        }

        [Test]
        public async Task RegisterAsync_FailedRegistration_ReturnsFalse()
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri == new Uri("http://127.0.0.1:5000/auth/register")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var result = await _apiClient.RegisterAsync("test@example.com", "testuser", "password123");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task LoginAsync_SuccessfulLogin_ReturnsTokenAndUserId()
        {
            var tokenData = new { access_token = "dummy_token", user_id = "123" };
            var jsonResponse = JsonConvert.SerializeObject(tokenData);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            };
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri == new Uri("http://127.0.0.1:5000/auth/login")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var (isSuccess, token, userId) = await _apiClient.LoginAsync("test@example.com", "password123");
            Assert.IsTrue(isSuccess);
            Assert.AreEqual("dummy_token", token);
            Assert.AreEqual("123", userId);
        }

        [Test]
        public async Task LoginAsync_FailedLogin_ReturnsFalse()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri == new Uri("http://127.0.0.1:5000/auth/login")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var (isSuccess, token, userId) = await _apiClient.LoginAsync("test@example.com", "password123");

            Assert.IsFalse(isSuccess);
            Assert.IsNull(token);
            Assert.IsNull(userId);
        }

        [Test]
        public async Task AddPostAsync_SuccessfulPostAddition_ReturnsTrue()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri == new Uri("http://127.0.0.1:5000/posts")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var result = await _apiClient.AddPostAsync("Test Title", "Test Content", "author123");

            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddPostAsync_FailedPostAddition_ReturnsFalse()
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri == new Uri("http://127.0.0.1:5000/posts")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            // Act
            var result = await _apiClient.AddPostAsync("Test Title", "Test Content", "author123");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddPostAsync_NullOrEmptyParameters_ReturnsFalse()
        {
            Assert.IsFalse(await _apiClient.AddPostAsync(null, "Test Content", "author123"));
            Assert.IsFalse(await _apiClient.AddPostAsync("Test Title", null, "author123"));
            Assert.IsFalse(await _apiClient.AddPostAsync("Test Title", "Test Content", null));
            Assert.IsFalse(await _apiClient.AddPostAsync("", "Test Content", "author123"));
            Assert.IsFalse(await _apiClient.AddPostAsync("Test Title", "", "author123"));
            Assert.IsFalse(await _apiClient.AddPostAsync("Test Title", "Test Content", ""));
        }
    }
}

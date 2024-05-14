using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace shop_desktop.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://bd73-82-139-13-67.ngrok-free.app/"; 
        public ApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<bool> RegisterAsync(string email, string username, string password)
        {
            var registerData = new { email = email, username = username, password = password };
            string jsonData = JsonConvert.SerializeObject(registerData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("/auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<(bool, string, string)> LoginAsync(string email, string password)
        {
            var loginData = new { email = email, password = password };
            string jsonData = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                var token = tokenData.access_token;
                var userId = tokenData.user_id;
                
                Console.WriteLine($"Token: {token}, UserID: {userId}");
                
                return (true, token, userId);
            }
            else
            {
                return (false, null, null);
            }
        }
        public async Task<bool> AddPostAsync(string title, string content, string authorId)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content) || string.IsNullOrEmpty(authorId))
            {
                Console.WriteLine("Błąd: title, content i authorId nie mogą być puste ani null.");
                return false;
            }
            var postData = new
            {
                title = title,
                content = content,
                author_id = authorId
            };

            string postJson = JsonConvert.SerializeObject(postData);
            HttpContent contentData = new StringContent(postJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("/posts", contentData);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
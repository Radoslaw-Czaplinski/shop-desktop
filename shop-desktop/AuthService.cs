using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace shop_desktop
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://example.com/api/"); // Adres Twojego API
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> RegisterAsync(string email, string password)
        {
            try
            {
                var registerData = new { Email = email, Password = password };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(registerData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync("register", content); // Endpoint do rejestracji użytkownika

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while registering: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                var loginData = new { Email = email, Password = password };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync("login", content); // Endpoint do logowania użytkownika

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while logging in: " + ex.Message);
                return false;
            }
        }
    }
}

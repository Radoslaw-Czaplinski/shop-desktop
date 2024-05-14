using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json; 
using System.Text; 
using System.Net.Http.Headers; 
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace shop_desktop.Services
{
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;
        public bool IsLoggedIn { get; private set; }
        private string _accessToken;
        public string AccessToken
        { 
            get => _accessToken; 
            set => _accessToken = value;
        }
        public string UserId { get; private set; }
        public AuthenticationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        private string DecodeJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var userId = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

            if (userId == null)
            {
                throw new Exception("UserID not found in token.");
            }

            return userId;
        }
        public async Task<(bool, string, string)> LoginAsync(string email, string password)
        {
            try
            {
                var loginData = new { email = email, password = password };
                string jsonData = JsonConvert.SerializeObject(loginData);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("/auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var tokenData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    var token = (string)tokenData.access_token;
                    var userId = DecodeJwtToken(token);

                    AccessToken = token;
                    UserId = userId;
                    IsLoggedIn = true;

                    return (true, token, userId);
                }
                else
                {
                    return (false, null, null);
                }
            }
            catch (Exception ex)
            {
                return (false, null, null);
            }
        }
        public void Logout()
        {
            IsLoggedIn = false;
            AccessToken = null;
            UserId = null;
        }
    }
}
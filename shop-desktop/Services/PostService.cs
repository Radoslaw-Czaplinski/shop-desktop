using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using shop_desktop.Models;
using System.Net.Http.Json;


namespace shop_desktop.Services
{
    public class PostService : INotifyPropertyChanged
    {
        private ObservableCollection<Post> _posts = new ObservableCollection<Post>();
        private ObservableCollection<Comment> _comments = new ObservableCollection<Comment>();
        private readonly HttpClient _httpClient;
        private readonly AuthenticationService _authenticationService;
        private readonly string _baseUrl = "https://bd73-82-139-13-67.ngrok-free.app/";

        public PostService(HttpClient httpClient, AuthenticationService authenticationService, string baseUrl)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            _baseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));

            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public ObservableCollection<Post> Posts
        {
            get { return _posts; }
            private set
            {
                if (_posts != value)
                {
                    _posts = value;
                    OnPropertyChanged(nameof(Posts));
                }
            }
        }
        public ObservableCollection<Comment> Comments
        {
            get { return _comments; }
            private set
            {
                if (_comments != value)
                {
                    _comments = value;
                    OnPropertyChanged(nameof(Comments));
                }
            }
        }
        public void SetAuthorizationHeader(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine($"Authorization header set with token: {token}");
            }
            else
            {
                Console.WriteLine("Received empty token.");
            }
        }
        public async Task<ObservableCollection<Post>> LoadPostsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/posts");
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var fetchedPosts = JsonConvert.DeserializeObject<ObservableCollection<Post>>(responseData, new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat
                });
                return fetchedPosts;
            }
            else
            {
                return new ObservableCollection<Post>();
            }
        }
        public async Task<List<Comment>> LoadCommentsAsync(int postId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"/posts/{postId}/comments");
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var comments = JsonConvert.DeserializeObject<List<Comment>>(responseData);
                return comments;
            }
            else
            {
                throw new Exception($"Failed to load comments: {response.StatusCode}");
            }
        }
        public async Task<List<Rating>> LoadRatingsAsync(int postId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"/posts/{postId}/ratings");
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var ratings = JsonConvert.DeserializeObject<List<Rating>>(responseData);
                return ratings;
            }
            else
            {
                throw new Exception($"Failed to load ratings: {response.StatusCode}");
            }
        }
        public async Task<bool> AddPostAsync(string title, string content, string authorId)
        {
            Console.WriteLine($"Authorization header set with token: {_httpClient.DefaultRequestHeaders.Authorization?.Parameter}");
            Console.WriteLine($"Sending post with Title: '{title}', Content: '{content}', AuthorID: '{authorId}'");

            if (string.IsNullOrEmpty(authorId))
            {
                Console.WriteLine("Author ID is null or empty.");
                return false;
            }

            var postData = new { title = title, content = content, author_id = authorId };
            string postJson = JsonConvert.SerializeObject(postData);
            var contentData = new StringContent(postJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("/posts", contentData);

            if (response.IsSuccessStatusCode)
            {
                await LoadPostsAsync();
                return true;
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to add post: {response.StatusCode}, Content: {responseContent}");
                return false;
            }
        }
        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            var updateData = new
            {
                title = postToUpdate.Title,
                content = postToUpdate.Content,
                author_id = postToUpdate.AuthorId
            };
            string jsonData = JsonConvert.SerializeObject(updateData);
            var contentData = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync($"/posts/{postToUpdate.Id}", contentData);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                Console.WriteLine($"Failed to update post: {response.StatusCode}");
                return false;
            }
        }
        public async Task<bool> DeletePostAsync(int postId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"/posts/{postId}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                Console.WriteLine($"Failed to delete post: {response.StatusCode}");
                return false;
            }
        }
        public async Task AddRatingAsync(int postId, int ratingValue)
        {
            try
            {
                var ratingData = new { score = ratingValue };
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"posts/{postId}/rate", ratingData);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"Wystąpił błąd podczas dodawania oceny: {ex.Message}");
            }
        }
        public async Task<bool> AddCommentAsync(int postId, object commentData)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"posts/{postId}/comments", commentData);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task LoadCommentsForPostAsync(int postId)
        {
            try
            {
                var comments = await LoadCommentsAsync(postId);
                Comments = new ObservableCollection<Comment>(comments);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load comments: {ex.Message}");
            }
        }
        public async Task<Post> GetPostDetailsAsync(int postId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"/posts/{postId}");
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var postDetails = JsonConvert.DeserializeObject<Post>(responseData);
                return postDetails;
            }
            else
            {
                throw new Exception($"Failed to get post details: {response.StatusCode}");
            }
        }
        public async Task PostAsync(string subject, string message)
        {
            try
            {
                var formData = new 
                {
                    subject = subject,
                    message = message
                };

                string jsonData = JsonConvert.SerializeObject(formData);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("/contact_author", content);
                response.EnsureSuccessStatusCode();

                MessageBox.Show("Formularz kontaktowy został pomyślnie wysłany.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas wysyłania formularza kontaktowego: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public async Task ReportIssueAsync(string title, string description)
        {
            try
            {
                var issueData = new { title = title, description = description };
                string jsonData = JsonConvert.SerializeObject(issueData);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("/report_issue", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Issue reported successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to report issue. Status code: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while reporting issue: " + ex.Message);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
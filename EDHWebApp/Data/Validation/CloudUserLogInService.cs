using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EDHWebApp.Model;

namespace Client.Data.Validation
{
    public class CloudUserLogInService : IUserLogInService
    {
        /// <summary>
        /// Uri of the 2nd tier server.
        /// </summary>
        private const string Uri = "https://localhost:7213";

        /// <summary>
        /// HttpClient used for making http requests to the 2nd tier server.
        /// </summary>
        private readonly HttpClient _httpClient;

        private string loggedInRole;
        /// <summary>
        /// Constructor dependency injection.
        /// </summary>
        public CloudUserLogInService()
        {
            _httpClient = new HttpClient();
        }

        /// <inheritdoc />
        public async Task<User> ValidateUserAsync(string username, string password)
        {
            
            var endUser = new User()
            {
                UserName = username,
                Password = password,
                Name = "string",
                Surname = "string",
                Email = "user@example.com",
                MyCompany = new Company()
                {
                    Name = "string",
                    Email = "user@example.com"
                },
                Role = "string",
                IsAdmin = true,
                VerifiedUser = true
            };
        
            var userAsJson = JsonSerializer.Serialize(endUser, new JsonSerializerOptions
            {
              PropertyNameCaseInsensitive  = true
            });

            
            HttpContent httpContent = new StringContent(
                userAsJson,
                Encoding.UTF8,
                "application/json");
            
            
            
            var responseMessage = await _httpClient.PostAsync(Uri + "/account", httpContent);

            
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception($"Error, {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");
            }

            var message = await responseMessage.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<User>(message);

            return result;
        }

        public void setLoggedInRole(string cachedUserRole)
        {
            loggedInRole = cachedUserRole;
        }

        public string getLoggedInRole()
        {
            return loggedInRole;
        }
    }
}
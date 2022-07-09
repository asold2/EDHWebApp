using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EDHWebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using JsonSerializer = System.Text.Json.JsonSerializer;


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
        private CookieContainer cookies;
        private int loggedInId;
        /// <summary>
        /// Constructor dependency injection.
        /// </summary>
        public CloudUserLogInService()
        {
            cookies = new CookieContainer();
             HttpClientHandler _clientHandler = new HttpClientHandler();
             _clientHandler.UseCookies = true;
             _clientHandler.CookieContainer = cookies;
             _httpClient = new HttpClient(_clientHandler);

             MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
             _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
        }

        /// <inheritdoc />
        public async Task<CompanyUser> ValidateUserAsync(string username, string password, bool rememberMe)
        {
        
            var endUser = new Account()
            {
                Username = username,
                Password = password,
                userId = 0,
                rememberMe = rememberMe
            };
        
            var userAsJson = JsonSerializer.Serialize(endUser, new JsonSerializerOptions
            {
              PropertyNameCaseInsensitive  = true
            });

            
            HttpContent httpContent = new StringContent(
                userAsJson,
                Encoding.UTF8,
                "application/json");


            ;
            var responseMessage = await _httpClient.PostAsync(Uri + "/account", httpContent);
            
        
            
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception($"Error, {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");
            }

            var message = await responseMessage.Content.ReadAsStringAsync();

            
            var result = JsonSerializer.Deserialize<CompanyUser>(message);



            loggedInId = result.UserId;
            loggedInRole = result.Role;
            
            
            return result;
        }


        public  async Task<ClaimsIdentity> getClaimsForUserAsync(CompanyUser companyUser)
        {
           
        
            var userAsJson = JsonSerializer.Serialize(companyUser, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive  = true
            });

            
            HttpContent httpContent = new StringContent(
                userAsJson,
                Encoding.UTF8,
                "application/json");

            

            
            
            HttpResponseMessage responseMessage = await _httpClient.PostAsync(Uri + "/claims", httpContent);
            

            Uri uri = new Uri(Uri+"/claims");
            IEnumerable<Cookie> responseCookies = cookies.GetCookies(uri).Cast<Cookie>();

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception($"Error, {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");
            }

            var message = await responseMessage.Content.ReadAsStringAsync();

            
            var result = JsonSerializer.Deserialize<ClaimsIdentity>(message);

            GetCookiesFromApi();
            
            return result;
            

        }

        public void GetCookiesFromApi()
        {
            HttpResponseMessage getResponse = _httpClient.GetAsync(Uri + "/cookie").Result;
            string message = JsonConvert.DeserializeObject<string>(getResponse.Content.ReadAsStringAsync().Result);
        }



        public async Task<HttpResponseMessage> Logout()
        {
            HttpContent httpContent = new StringContent(
                "",
                Encoding.UTF8,
                "application/json");

            
            
            return await _httpClient.PostAsync(Uri+"/logout", httpContent );
        }


        public void setLoggedInRole(string cachedUserRole)
        {
            loggedInRole = cachedUserRole;
        }

        public string getLoggedInRole()
        {
            return loggedInRole;
        }

 
        public void setLoggedInId(int id)
        {
            loggedInId = id;
        }

        public int getLoggedInId()
        {
            return loggedInId;
        }
    }
}
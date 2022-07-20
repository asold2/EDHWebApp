using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Client.Data.Validation;
using EDHWebApp.Data.TokenData;
using EDHWebApp.Model;
using Hanssens.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace EDHWebApp.Authentication
{
    /// <summary>
    /// A class used for authenticating logged in user.
    /// </summary>
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IUserLogInService _logInService;
        // private readonly ILocalStorageService _localStorageService;
        private readonly ITokenManager _tokenManager;
        
            

        
        
        
        private CompanyUser _cachedCompanyUser = new CompanyUser();
        
        
        
        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime, IUserLogInService logInService, ITokenManager tokenManager)
        {
            _jsRuntime = jsRuntime;
            _logInService = logInService;
            _cachedCompanyUser.Role = "";
            _cachedCompanyUser.UserId = 0;
            // _localStorageService = localStorage;
            _tokenManager = tokenManager;
        }
        
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            if (_cachedCompanyUser == null)
            {
                var userAsJson = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
                if (!string.IsNullOrEmpty(userAsJson))
                {
                    _cachedCompanyUser = JsonSerializer.Deserialize<CompanyUser>(userAsJson);
                    identity = SetupClaimsForUser(_cachedCompanyUser);
                }
            }
            else
            {
                identity = SetupClaimsForUser(_cachedCompanyUser);

            }

            var cachedClaimsPrincipal = new ClaimsPrincipal(identity);
        
            return await Task.FromResult(new AuthenticationState(cachedClaimsPrincipal));
        }

        public async Task ValidateLogin(string username, string password, bool rememberMe)
        {
            if (string.IsNullOrEmpty(username)) throw new Exception("Enter username");
            if (string.IsNullOrEmpty(password)) throw new Exception("Enter password");


            Console.WriteLine("Validating");
            var user = await _logInService.ValidateUserAsync(username, password, rememberMe);




            var identity = SetupClaimsForUser(user);

           
            
            
            var serialisedData = JsonSerializer.Serialize(user);
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serialisedData);
            _cachedCompanyUser = user;
            
            setLoggedInId(_cachedCompanyUser.UserId);
            setLoggedInRole(_cachedCompanyUser.Role);
            _logInService.SetCompanyUserName(_cachedCompanyUser.Name + " " + _cachedCompanyUser.Surname);

            
            await _tokenManager.SetRefreshToke(user.RefreshToken);
            
          
            
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
           
        }
        
        
        
        private ClaimsIdentity SetupClaimsForUser(CompanyUser endCompanyUser)
        {
        
            string isVerified = "";
            if (endCompanyUser.VerifiedUser)
            {
                isVerified = "isVerified";
            }
        
            var claims = new List<Claim> {new("Role",  endCompanyUser.Role), new ("isVerified", isVerified), new (ClaimTypes.Name, endCompanyUser.Username)};
        
    
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
         
            return identity;

        }
        
        void setLoggedInRole(string role)
        {
            _logInService.setLoggedInRole(role);
        }

        void setLoggedInId(int id)
        {
            _logInService.setLoggedInId(id);
        }
        
        public async Task Logout()
        {
            _cachedCompanyUser = null;
            var user = new ClaimsPrincipal(new ClaimsIdentity());

            await _logInService.Logout();
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }


        public CompanyUser getCachedUser()
        {
            return _cachedCompanyUser;
        }


    }
    
    
  
    }

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Client.Data.Validation;
using EDHWebApp.Model;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Client.Authentication
{
    /// <summary>
    /// A class used for authenticating logged in user.
    /// </summary>
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IUserLogInService _logInService;

        
        
        
        private User _cachedUser = new User();
        
        
        
        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime, IUserLogInService logInService)
        {
            _jsRuntime = jsRuntime;
            _logInService = logInService;
            _cachedUser.Role = "";
        }
        
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            if (_cachedUser == null)
            {
                var userAsJson = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
                if (!string.IsNullOrEmpty(userAsJson))
                {
                    _cachedUser = JsonSerializer.Deserialize<User>(userAsJson);
                    identity = SetupClaimsForUser(_cachedUser);
                }
            }
            else
            {
                identity = SetupClaimsForUser(_cachedUser);
            }

            var cachedClaimsPrincipal = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(cachedClaimsPrincipal));
        }

        public async Task ValidateLogin(string username, string password)
        {
            if (string.IsNullOrEmpty(username)) throw new Exception("Enter username");
            if (string.IsNullOrEmpty(password)) throw new Exception("Enter password");
            
            var user = await _logInService.ValidateUserAsync(username, password);
            var identity = SetupClaimsForUser(user);
            
            var serialisedData = JsonSerializer.Serialize(user);
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serialisedData);
            _cachedUser = user;
            
            getLoggedInRole();
            
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
        }
        
        
        private ClaimsIdentity SetupClaimsForUser(User endUser)
        {
            string isVerified = "";
            if (endUser.VerifiedUser)
            {
                isVerified = "isVerified";
            }

            var claims = new List<Claim> {new("Role",  endUser.Role), new ("isVerified", isVerified)};

            //Add a claim to check for the type of the subclass(EndUser)
            var identity = new ClaimsIdentity(claims, "apiauth_type");
            return identity;
        }

        void getLoggedInRole()
        {
            _logInService.setLoggedInRole(_cachedUser.Role);
        }
        
        public async Task Logout()
        {
            _cachedUser = null;
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
        
        
    }
    
    
  
    }

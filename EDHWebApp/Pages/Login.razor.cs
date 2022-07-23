using System;
using System.Net;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Web;



using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Client.Data.Validation;
using Microsoft.AspNetCore.Components.Web;
using System.Data.SqlClient;
using EDHWebApp.Authentication;
using EDHWebApp.Data.TokenData;
using EDHWebApp.Model;


namespace EDHWebApp.Pages
{
    /// <summary>
    /// C# code for login page.
    /// </summary>
    public class LoginRazor : ComponentBase
    {

        /// <summary>
        /// Injected NavigationManager for navigating through pages.
        /// </summary>
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Injected AuthenticationStateProvider for validating username and password.
        /// </summary>
        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; }


        [Inject] private IUserLogInService LogInService { get; set; }

        /// <summary>
        /// Bound username of type string.
        /// </summary>
        protected string Username = "";


        protected bool RememberMe = false;

        /// <summary>
        /// Bound password of type string.
        /// </summary>
        protected string Password = "";

        /// <summary>
        /// Bound error message of type string shown when either password or username is blank, or username and password
        /// does not exists.
        /// </summary>
        protected string ErrorMessage;

        /// <summary>
        /// Method used for navigating to the main page, when the username and password has been verified.
        /// </summary>
        ///


        
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await ((CustomAuthenticationStateProvider) AuthenticationStateProvider).AuthenticateByToken();
                if (LogInService.getLoggedInRole() == "User")
                {
                    NavigationManager.NavigateTo("/UserView");
                }
                else if (LogInService.getLoggedInRole() == "Admin")
                {
                    NavigationManager.NavigateTo("/ViewCompanies");
                }
            }
            catch (Exception e) {
                NavigationManager.NavigateTo("/");
            }

        }
        


        protected async Task LoadMainPage()
        {
            ErrorMessage = "";
            try
            {
                await ((CustomAuthenticationStateProvider) AuthenticationStateProvider).ValidateLogin(Username,
                    Password, RememberMe);
         
                
                if (LogInService.getLoggedInRole() == "User")
                {
                    NavigationManager.NavigateTo("/UserView");
                }
                else if (LogInService.getLoggedInRole() == "Admin")
                {
                    NavigationManager.NavigateTo("/ViewCompanies");
                }
            }
            catch (Exception)
            {
                ErrorMessage = "Username or Password are incorrect. Please, Try again.";
            }






        }

        protected void About()
        {
            NavigationManager.NavigateTo("/About");
        }

        /// <summary>
        /// Method used for navigating to the create account page
        /// </summary>
        protected void LoadCreateAccount()
        {
            NavigationManager.NavigateTo("Register");
        }

        /// <summary>
        /// Method for giving functionality to the "Enter" keyboard button.
        /// </summary>
        /// <param name="e"></param>
        protected async Task Enter(KeyboardEventArgs e)
        {
            if (e.Code is "Enter" or "NumpadEnter")
            {
                await LoadMainPage();

            }
        }

        protected void OutlookLogin()
        {
            Console.WriteLine("Not working yet");
        }


    }
}
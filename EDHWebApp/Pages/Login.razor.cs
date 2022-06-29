using System;
using System.Threading.Tasks;


using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Client.Authentication;
using Client.Data.Validation;
using Microsoft.AspNetCore.Components.Web;





namespace Client.Pages
{
    /// <summary>
    /// C# code for login page.
    /// </summary>
    public class LoginRazor : ComponentBase
    {
        /// <summary>
        /// Injected NavigationManager for navigating through pages.
        /// </summary>
        [Inject] private NavigationManager NavigationManager { get; set; }
        /// <summary>
        /// Injected AuthenticationStateProvider for validating username and password.
        /// </summary>
        [Inject] private AuthenticationStateProvider AuthenticationStateProvider{ get; set; }

        [Inject] private IUserLogInService LogInService { get; set; }

        /// <summary>
        /// Bound username of type string.
        /// </summary>
        protected string Username = "";
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
        protected async Task LoadMainPage()
    {
        ErrorMessage = "";
        try
        {
            await ((CustomAuthenticationStateProvider) AuthenticationStateProvider).ValidateLogin(Username, Password);
            
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
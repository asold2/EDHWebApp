using System.Security.Claims;
using System.Threading.Tasks;
using EDHWebApp.Model;
using Microsoft.AspNetCore.Mvc;

namespace Client.Data.Validation
{
    /// <summary>
    /// A login service interface
    /// </summary>
    public interface IUserLogInService 
    {
        /// <summary>
        /// Method used for validating login information.
        /// </summary>
        /// <param name="username">Given username</param>
        /// <param name="password">Given password</param>
        /// <returns>An object of an EndUser will all information if successfully validated</returns>
        Task<CompanyUser> ValidateUserAsync(string username, string password, bool rememberMe);

        void setLoggedInRole(string cachedUserRole);
        string getLoggedInRole();
        void setLoggedInId(int id);

        int getLoggedInId();
        Task<ClaimsIdentity> getClaimsForUserAsync(CompanyUser companyUser);
        Task<HttpResponseMessage> Logout();
        string GetCurrentUserName();
        void SetCompanyUserName(string userName);
    }
}
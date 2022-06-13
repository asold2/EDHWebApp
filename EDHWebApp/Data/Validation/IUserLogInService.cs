using System.Threading.Tasks;
using EDHWebApp.Model;

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
        Task<User> ValidateUserAsync(string username, string password);

        void setLoggedInRole(string cachedUserRole);
        string getLoggedInRole();
    }
}
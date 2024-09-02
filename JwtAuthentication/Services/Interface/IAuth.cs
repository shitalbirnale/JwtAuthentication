using JwtAuthentication.Model;

namespace JwtAuthentication.Services.Interface
{
    public interface IAuth
    {
        /// <summary>
        /// LogIn Validate Login User
        /// </summary>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        Task<string> LogIn(LoginDTO userDetails);
    }
}

using JwtAuthentication.Constants;
using JwtAuthentication.Model;
using JwtAuthentication.Services.Interface;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthentication.Services.Implementation
{
    /// <summary>
    /// AuthService
    /// </summary>
    public class AuthService : IAuth
    {
        /// <summary>
        /// Injetable Services
        /// </summary>
        private readonly IEmployee _employeeService;
        private readonly JwtOptions _jwtOptions;
        public AuthService(IEmployee employee, IOptions<JwtOptions> jwtOptions)
        {
            _employeeService = employee;
            _jwtOptions = jwtOptions.Value;
        }

        /// <summary>
        /// LogIn Validate Login User
        /// </summary>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        public async Task<string> LogIn(LoginDTO userDetails)
        {
            var result = string.Empty;
            try
            {
                var employee = await _employeeService.GetEmployeeByEmail(userDetails.Email);
                if (employee is null)
                {
                   return result = ValidationConstant.EMAIL_NOT_EXIST;
                }
                if (employee.Password != userDetails.Password)
                {
                   return result = ValidationConstant.EMAILPASS_INCORRECT;
                }
                var jwtKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Key));
                var credential = new SigningCredentials(jwtKey, SecurityAlgorithms.HmacSha256);
                List<Claim> claims = new List<Claim>() { new Claim(ClaimConstant.EMAIL, userDetails.Email) };
                var securityToken = new JwtSecurityToken(_jwtOptions.Key, _jwtOptions.Issuer, claims, expires: DateTime.Now.AddHours(1), signingCredentials: credential);
                var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
                result = ClaimConstant.BEARER + " " + token;
            }
            catch (Exception ex) 
            {
                result = ex.Message;
            }
            return result;
        }
    }
}

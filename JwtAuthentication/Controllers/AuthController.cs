using Asp.Versioning;
using JwtAuthentication.Model;
using JwtAuthentication.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthentication.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IEmployee _employeeService;
        private readonly JwtOptions _jwtOptions;
        public AuthController(IEmployee employee,IOptions<JwtOptions> jwtOptions)
        {
            _employeeService = employee;
            _jwtOptions = jwtOptions.Value;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO userDetails)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByEmail(userDetails.Email);
                if (employee is null || (employee != null && employee.Password != userDetails.Password))
                {
                    return BadRequest(new { error = "Email/Password is not correct" });
                }
                var jwtKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Key));
                var credential = new SigningCredentials(jwtKey, SecurityAlgorithms.HmacSha256);
                List<Claim> claims = new List<Claim>() { new Claim("Email", userDetails.Email) };
                var securityToken = new JwtSecurityToken(_jwtOptions.Key, _jwtOptions.Issuer, claims, expires: DateTime.Now.AddMinutes(5), signingCredentials: credential);
                var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
                return Ok("Bearer " + token);
            }
            catch (Exception ex) 
            { 
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

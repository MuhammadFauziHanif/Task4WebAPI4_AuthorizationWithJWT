using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;
using MyRESTServices.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyBackendServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IUserBLL _userBLL;

        public AccountsController(IOptions<AppSettings> appSettings, IUserBLL userBLL)
        {
            _appSettings = appSettings.Value;
            _userBLL = userBLL;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            try
            {
                var user = await _userBLL.Login(loginDto);
                if (user != null)
                {
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, loginDto.Username));

                    var roles = await _userBLL.GetUserWithRoles(loginDto.Username);
                    foreach (var role in roles.Roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
                    }
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.Now.AddHours(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var userWithToken = new UserWithTokenDTO
                    {
                        Username = loginDto.Username,
                        Password = loginDto.Password,
                        Token = tokenHandler.WriteToken(token)
                    };
                    return Ok(userWithToken);
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

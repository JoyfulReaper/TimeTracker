using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TimeTrackerApi.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace TimeTrackerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _config;

    public AuthController(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
    }

    [HttpPost("signin")]
    [ProducesResponseType(typeof(SignInResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(SignInResponse), StatusCodes.Status200OK)]
    public async Task<object> SignIn([FromBody] SignInCredentials creds)
    {
        IdentityUser user = await _userManager.FindByNameAsync(creds.Username);
        if (user != null)
        {
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, creds.Password, true);

            if (result.Succeeded)
            {
                SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
                {
                    Subject = (await _signInManager.CreateUserPrincipalAsync(user))
                        .Identities.First(),
                    Expires = DateTime.UtcNow.AddMinutes(
                        int.Parse(_config["BearerTokens:ExpiryMins"])),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_config["BearerTokens:Key"])),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                SecurityToken secToken = handler.CreateToken(descriptor);

                return new SignInResponse
                {
                    Success = true,
                    Token = handler.WriteToken(secToken),
                    Username = user.UserName
                };
            }
        }

        return StatusCode(StatusCodes.Status401Unauthorized, new SignInResponse
        {
            Success = false
        });
    }
}

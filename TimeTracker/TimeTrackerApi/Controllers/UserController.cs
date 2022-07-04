using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimeTrackerApi.Library.Models;
using TimeTrackerApi.Library.Repositories.Interfaces;

namespace TimeTrackerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserRepo _userRepo;

    public UserController(IUserRepo userRepo)
    {
        _userRepo = userRepo;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    public async Task<User> GetLoggedInUser()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _userRepo.GetUserByIdAsync(userId);
    }
}

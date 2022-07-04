using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimeTrackerApi.Library.Models;
using TimeTrackerApi.Library.Repositories.Interfaces;

namespace TimeTrackerApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class SubcategoryController : ControllerBase
{
    private readonly ISubcategoryRepo _subcategory;
    
    public SubcategoryController(ISubcategoryRepo subcategory)
    {
        _subcategory = subcategory;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Subcategory>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCategories(int categoryId)
    {
        var subCategories =  await _subcategory.GetSubcategoriesAsync(categoryId);

        if (subCategories.First().UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { ErrorReason = "You may only access your own categories" });
        }

        return Ok(subCategories);
    }

    [HttpPost]
    public async Task AddCategory(Subcategory subcategory)
    {
        await _subcategory.AddSubcategoryAsync(subcategory);
    }
}

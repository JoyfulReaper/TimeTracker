using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimeTrackerApi.DTOs;
using TimeTrackerApi.Library.Models;
using TimeTrackerApi.Library.Repositories.Interfaces;
using TimeTrackerApi.Models;

namespace TimeTrackerApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepo _category;

    public CategoryController(ICategoryRepo category)
    {
        _category = category;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCategories(string userId)
    {
        var output = new List<CategoryDTO>();

        var categories = await _category.GetCategoriesAsync(userId);
        if (categories.First().UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { ErrorReason = "You may only access your own categories" });
        }

        foreach (var c in categories)
        {
            output.Add(new CategoryDTO
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                UserId = c.UserId,
                DateCreated = c.DateCreated,
                ProjectsInCategory = await _category.GetProjectCount(c.CategoryId)
            });
        }
        
        return Ok(output);
    }

    [HttpPost]
    public async Task AddCategory(Category category)
    {
        await _category.AddCategoryAsync(category);
    }

    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> DeleteCategory(int categoryId)
    {
        try
        {
            await _category.DeleteCategoryAsync(categoryId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse { ErrorMessage = ex.Message});
        }
    }
}

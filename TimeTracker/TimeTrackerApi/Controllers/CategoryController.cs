using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTrackerApi.Library.Models;
using TimeTrackerApi.Library.Repositories.Interfaces;

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

    [HttpGet("category")]
    public async Task <IEnumerable<Category>> GetCategories(string userId)
    {
        return await _category.GetCategoriesAsync(userId);
    }
}

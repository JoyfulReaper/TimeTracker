﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Category>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCategories(string userId)
    {
        var categories =  await _category.GetCategoriesAsync(userId);
        if(categories.First().UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { ErrorReason = "You may only access your own categories" });
        }
        
        return Ok(categories);
    }

    [HttpPost]
    public async Task AddCategory(Category category)
    {
        await _category.AddCategoryAsync(category);
    }
}

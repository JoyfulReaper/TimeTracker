using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrackerWpf.Library.Models;
using System.Net.Http.Json;

namespace TimeTrackerWpf.Library.Api;
public class CategoryEndpoint : ICategoryEndpoint
{
    private readonly IApiClient _apiClient;
    private readonly ILoggedInUser _loggedInUser;

    public CategoryEndpoint(IApiClient apiClient,
        ILoggedInUser loggedInUser)
    {
        _apiClient = apiClient;
        _loggedInUser = loggedInUser;
    }

    public async Task<IEnumerable<Category>> GetCategories()
    {
        var categories = await _apiClient.Client.GetFromJsonAsync<IEnumerable<Category>>($"/api/category?userId={_loggedInUser.UserId}")
            ?? throw new Exception("Failed to deseraizlize API response");

        return categories;
    }

    public async Task AddCategory(Category category)
    {
        await _apiClient.Client.PostAsJsonAsync("/api/category", category);
    }
}

using TimeTrackerApi.Library.Models;

namespace TimeTrackerApi.Library.Repositories.Interfaces;
public interface ICategoryRepo
{
    Task<List<Category>> GetCategoriesAsync(string userId);
    Task SaveCategoryAsync(Category category);
}
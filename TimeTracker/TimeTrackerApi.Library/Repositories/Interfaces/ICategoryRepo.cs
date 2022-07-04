using TimeTrackerApi.Library.Models;

namespace TimeTrackerApi.Library.Repositories.Interfaces;
public interface ICategoryRepo
{
    Task<List<Category>> GetCategoriesAsync(string userId);
    Task AddCategoryAsync(Category category);
    Task DeleteCategoryAsync(int categoryId);
    Task<int> GetProjectCount(int categoryId);
}
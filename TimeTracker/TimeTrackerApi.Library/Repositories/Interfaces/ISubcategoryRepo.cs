using TimeTrackerApi.Library.Models;

namespace TimeTrackerApi.Library.Repositories.Interfaces;
public interface ISubcategoryRepo
{
    Task AddSubcategoryAsync(Subcategory subcategory);
    Task<List<Subcategory>> GetSubcategoriesAsync(int categoryId);
}
using TimeTrackerWpf.Library.Models;

namespace TimeTrackerWpf.Library.Api;
public interface ICategoryEndpoint
{
    Task AddCategory(Category category);
    Task<IEnumerable<Category>> GetCategories();
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrackerApi.Library.DataAccess;
using TimeTrackerApi.Library.Models;
using TimeTrackerApi.Library.Repositories.Interfaces;

namespace TimeTrackerApi.Library.Repositories;
public class CategoryRepo : ICategoryRepo
{
    private readonly IDataAccess _dataAccess;

    public CategoryRepo(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public Task<List<Category>> GetCategoriesAsync(string userId)
    {
        return _dataAccess.LoadDataAsync<Category, dynamic>("spCategory_Get", new { UserId = userId }, "TimeTrackerData");
    }

    public Task<Category> GetCategory(int categoryId)
    {
        throw new NotImplementedException();
    }

    public Task AddCategoryAsync(Category category)
    {
        return _dataAccess.SaveDataAsync("spCategory_Insert", new
        {
            UserId = category.UserId,
            Name = category.Name
        }, "TimeTrackerData");
    }

    public async Task<int> GetProjectCount(int categoryId)
    {
        return (await _dataAccess.QueryRawSql<int, dynamic>("SELECT COUNT(*) FROM Project WHERE CategoryId = @CategoryId", 
            new { CategoryId = categoryId }, "TimeTrackerData")).Single();
    }

    public Task DeleteCategoryAsync(int categoryId)
    {
        return _dataAccess.SaveDataAsync("spCategory_Delete", new { CategoryId = categoryId }, "TimeTrackerData");
    }
}

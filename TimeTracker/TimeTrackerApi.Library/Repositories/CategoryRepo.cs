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

    public Task AddCategoryAsync(Category category)
    {
        return _dataAccess.SaveDataAsync("spCategory_Insert", category, "TimeTrackerData");
    }
}

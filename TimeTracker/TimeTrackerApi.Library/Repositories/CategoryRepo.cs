using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrackerApi.Library.DataAccess;
using TimeTrackerApi.Library.Models;

namespace TimeTrackerApi.Library.Repositories;
public class CategoryRepo
{
    private readonly IDataAccess _dataAccess;

    public CategoryRepo(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public Task<List<Category>> GetCategorys(string userId)
    {
        return _dataAccess.LoadDataAsync<Category, dynamic>("spCategory_Get", new { UserId = userId }, "TimeTrackerData");
    }

    public Task SaveCategory(Category category)
    {
        return _dataAccess.SaveDataAsync("spCategory_Save", category, "TimeTrackerData");
    }
}

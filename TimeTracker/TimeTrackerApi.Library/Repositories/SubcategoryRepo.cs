using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrackerApi.Library.DataAccess;
using TimeTrackerApi.Library.Models;
using TimeTrackerApi.Library.Repositories.Interfaces;

namespace TimeTrackerApi.Library.Repositories;
public class SubcategoryRepo : ISubcategoryRepo
{
    private readonly IDataAccess _dataAccess;

    public SubcategoryRepo(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public Task<List<Subcategory>> GetSubcategoriesAsync(int categoryId)
    {
        return _dataAccess.LoadDataAsync<Subcategory, dynamic>("spSubcategory_Get", new
        {
            CategoryId = categoryId
        },
        "TimeTrackerData");
    }

    public async Task AddSubcategoryAsync(Subcategory subcategory)
    {
        await _dataAccess.SaveDataAsync("spSubcategory_Insert", new
        {
            CategoryId = subcategory.CategoryId,
            Name = subcategory.Name,
            UserId = subcategory.UserId
        }, "TimeTrackerData");
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrackerApi.Library.DataAccess;
using TimeTrackerApi.Library.Models;
using TimeTrackerApi.Library.Repositories.Interfaces;

namespace TimeTrackerApi.Library.Repositories;
public class UserRepo : IUserRepo
{
    private readonly IDataAccess _dataAccess;
    
    public UserRepo(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        var output = await _dataAccess.LoadDataAsync<User, dynamic>("spUser_GetById", new { UserId = userId }, "TimeTrackerData");
        return output.Single();
    }
}

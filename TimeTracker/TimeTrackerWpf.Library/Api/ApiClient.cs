using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrackerWpf.Library.Models;

namespace TimeTrackerWpf.Library.Api;

public class ApiClient : IApiClient
{
    private readonly IConfiguration _config;

    private HttpClient _apiClient { get; set; }

    public HttpClient Client {
        get => _apiClient;
    }
    public ILoggedInUser LoggedInUser { get; }

    public ApiClient(ILoggedInUser loggedInUser,
        IConfiguration config)
    {
        LoggedInUser = loggedInUser;
        _config = config;

        Initialize();
    }

    private void Initialize()
    {
        string api = _config["api"];
    }

    public Task<AuthenticatedUser> Authenticate(string username, string password)
    {
        throw new NotImplementedException();
    }

    public void LogOffUser()
    {
        throw new NotImplementedException();
    }

    public Task GetLoggedInUserInfo(string token)
    {
        throw new NotImplementedException();
    }
}

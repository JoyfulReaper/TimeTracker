using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TimeTrackerWpf.Library.Models;

namespace TimeTrackerWpf.Library.Api;

public class ApiClient : IApiClient
{
    private readonly ILoggedInUser _loggedInUser;
    private readonly IConfiguration _config;

    private HttpClient _apiClient { get; set; } = null!;

    public HttpClient Client
    {
        get => _apiClient;
    }

    public ApiClient(ILoggedInUser loggedInUser,
        IConfiguration config)
    {
        _loggedInUser = loggedInUser;
        _config = config;

        Initialize();
    }

    private void Initialize()
    {
        string api = _config["api"];

        _apiClient = new HttpClient();
        _apiClient.BaseAddress = new Uri(api);
        _apiClient.DefaultRequestHeaders.Accept.Clear();
        _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<AuthenticatedUser> Authenticate(string username, string password)
    {
        using HttpResponseMessage response = await _apiClient.PostAsJsonAsync("api/auth/signin", new { Username = username, Password = password });
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<AuthenticatedUser>()
                ?? throw new Exception("Unable to de-seialize API response");

            _apiClient.DefaultRequestHeaders.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {result.Token}");

            await GetLoggedInUserInfo(result.Token);
            
            return result;
        }
        else
        {
            throw new Exception(response.ReasonPhrase);
        }
    }

    public void LogOffUser()
    {
        _apiClient.DefaultRequestHeaders.Clear();
    }
    
    private async Task GetLoggedInUserInfo(string token)
    {
        var user = await _apiClient.GetFromJsonAsync<LoggedInUser>("api/User");
        if(user == null)
        {
            throw new Exception("Unable to deserialze API response");
        }

        _loggedInUser.UserId = user.UserId;
        _loggedInUser.Token = token;
        _loggedInUser.FirstName = user.FirstName;
        _loggedInUser.LastName = user.LastName;
        _loggedInUser.EmailAddress = user.EmailAddress;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrackerWpf.Library.Models;

namespace TimeTrackerWpf.Library.Api;
public interface IApiClient
{
    HttpClient Client { get; }
    Task<AuthenticatedUser> Authenticate(string username, string password);
    void LogOffUser();
}

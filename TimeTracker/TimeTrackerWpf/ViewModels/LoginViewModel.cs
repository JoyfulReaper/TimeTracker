using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrackerWpf.Library.Api;

namespace TimeTrackerWpf.ViewModels;
public class LoginViewModel : Screen
{
    private readonly IApiClient _client;

    public LoginViewModel(IApiClient client)
    {
        _client = client;
    }
}

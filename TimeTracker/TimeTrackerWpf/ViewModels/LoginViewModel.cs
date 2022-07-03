using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrackerWpf.EventModels;
using TimeTrackerWpf.Library.Api;

namespace TimeTrackerWpf.ViewModels;
public class LoginViewModel : Screen
{
    private readonly IApiClient _client;
    private readonly IEventAggregator _eventAggregator;

    public LoginViewModel(IApiClient client,
        IEventAggregator eventAggregator)
    {
        _client = client;
        _eventAggregator = eventAggregator;
    }

    private string _username = "Admin";

    public string Username
    {
        get { return _username; }
        set
        {
            _username = value;
            NotifyOfPropertyChange(() => Username);
            NotifyOfPropertyChange(() => CanLogin);
        }
    }

    private string _password = "Pass123!";

    public string Password
    {
        get { return _password; }
        set
        {
            _password = value;
            NotifyOfPropertyChange(() => Password);
            NotifyOfPropertyChange(() => CanLogin);
        }
    }

    public bool IsErrorVisible
    {
        get
        {
            return ErrorMessage?.Length > 0;
        }
    }

    private string _errorMessage;

    public string ErrorMessage
    {
        get { return _errorMessage; }
        set
        {
            _errorMessage = value;
            NotifyOfPropertyChange(() => IsErrorVisible);
            NotifyOfPropertyChange(() => ErrorMessage);
        }
    }


    public bool CanLogin
    {
        get
        {
            if (Username?.Length > 0 && Password?.Length > 0)
            {
                return true;
            }
            return false;
        }
    }

    public async Task Login()
    {
        try
        {
            ErrorMessage = "";
            var result = await _client.Authenticate(Username, Password);
            await _eventAggregator.PublishOnUIThreadAsync(new LogOnEvent());
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}

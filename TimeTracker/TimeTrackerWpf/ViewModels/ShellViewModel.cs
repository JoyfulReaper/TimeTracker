using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TimeTrackerWpf.ViewModels;

public class ShellViewModel : Conductor<object>
{
    private readonly LoginViewModel _login;

    public ShellViewModel(LoginViewModel login)
    {
        _login = login;

        Task.Run(async () =>
        {
            await ActivateItemAsync(_login, new CancellationToken());
        });
        
    }
}

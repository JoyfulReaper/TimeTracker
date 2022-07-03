using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimeTrackerWpf.EventModels;
using TimeTrackerWpf.Library.Api;

namespace TimeTrackerWpf.ViewModels;

public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
{
    private readonly LoginViewModel _login;
    private readonly IEventAggregator _eventAggregator;
    private readonly IApiClient _apiClient;
    private readonly CategoryManagerViewModel _category;

    public ShellViewModel(LoginViewModel login,
        IEventAggregator eventAggregator,
        IApiClient apiClient,
        CategoryManagerViewModel category)
    {
        _login = login;
        _eventAggregator = eventAggregator;
        _apiClient = apiClient;
        _category = category;

        _eventAggregator.SubscribeOnPublishedThread(this);

        Task.Run(async () =>
        {
            await ActivateItemAsync(_login, new CancellationToken());
        });
    }

    public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
    {
        await ActivateItemAsync(_category, cancellationToken);
    }
}

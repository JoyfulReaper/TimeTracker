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
    private readonly CategoryManagerViewModel _categoryVm;

    public ShellViewModel(CategoryManagerViewModel categoryVm)
    {
        _categoryVm = categoryVm;
        
        Task.Run(async () =>
        {
            await ActivateItemAsync(_categoryVm, new CancellationToken());
        });
    }
}

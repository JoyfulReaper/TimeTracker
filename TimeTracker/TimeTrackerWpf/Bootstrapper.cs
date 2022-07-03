using Caliburn.Micro;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TimeTrackerWpf.Helpers;
using TimeTrackerWpf.Library.Api;
using TimeTrackerWpf.ViewModels;

namespace TimeTrackerWpf;
public class Bootstrapper : BootstrapperBase
{
    private SimpleContainer _container = null!;
    
    public Bootstrapper()
    {
        Initialize();

        ConventionManager.AddElementConvention<PasswordBox>(
            PasswordBoxHelper.BoundPasswordProperty,
            "Password",
            "PasswordChanged");
    }

    protected override void Configure()
    {
        IConfigurationBuilder configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true);
        IConfiguration config = configBuilder.Build();

        _container = new SimpleContainer();
        
        _container.Singleton<IWindowManager, WindowManager>();
        _container.Singleton<IEventAggregator, EventAggregator>();
        _container.Singleton<IApiClient, ApiClient>();
        _container.Instance(config);

        GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));
    }

  
    ///////////////////////Setup DI////////////////////////////////
    protected override object GetInstance(Type service, string key)
    {
        return _container.GetInstance(service, key);
    }

    protected override IEnumerable<object> GetAllInstances(Type service)
    {
        return _container.GetAllInstances(service);
    }

    protected override void BuildUp(object instance)
    {
        _container.BuildUp(instance);
    }
    ///////////////////////End Setup DI//////////////////////////////


    
    ///////////////////////Assembies to look for views in////////////
    protected override IEnumerable<Assembly> SelectAssemblies()
    {
        return new[] { Assembly.GetExecutingAssembly() };
    }


    
    protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
    {
        DisplayRootViewForAsync<ShellViewModel>();
    }
}

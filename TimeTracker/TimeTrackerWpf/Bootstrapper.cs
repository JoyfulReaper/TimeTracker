using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TimeTrackerWpf.ViewModels;

namespace TimeTrackerWpf;
public class Bootstrapper : BootstrapperBase
{
    private SimpleContainer _container;
    
    public Bootstrapper()
    {
        Initialize();
    }

    protected override void Configure()
    {
        _container = new SimpleContainer();

        _container.Singleton<IWindowManager, WindowManager>();
        _container.Singleton<IEventAggregator, EventAggregator>();

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

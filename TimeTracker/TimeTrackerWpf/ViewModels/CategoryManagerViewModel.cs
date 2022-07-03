using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrackerWpf.Library.Api;
using TimeTrackerWpf.Library.Models;

namespace TimeTrackerWpf.ViewModels;
public class CategoryManagerViewModel : Screen
{
    private readonly ICategoryEndpoint _categoryEndpoint;

    public CategoryManagerViewModel(ICategoryEndpoint categoryEndpoint)
    {
        _categoryEndpoint = categoryEndpoint;
    }

    private BindingList<Category> _categories;

    public BindingList<Category> Categories
    {
        get { return _categories; }
        set
        {
            _categories = value;
            NotifyOfPropertyChange(() => Categories);
        }
    }


    protected override async void OnViewLoaded(object view)
    {
        base.OnViewLoaded(view);
        await LoadCategories();
    }

    private async Task LoadCategories()
    {
        var categories = await _categoryEndpoint.GetCategories();
        Categories = new BindingList<Category>(categories.ToList());
    }
}

using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimeTrackerWpf.Library.Api;
using TimeTrackerWpf.Library.Models;

namespace TimeTrackerWpf.ViewModels;
public class CategoryManagerViewModel : Screen
{
    private readonly ICategoryEndpoint _categoryEndpoint;
    private readonly ILoggedInUser _loggedInUser;

    public CategoryManagerViewModel(ICategoryEndpoint categoryEndpoint,
        ILoggedInUser loggedInUser)
    {
        _categoryEndpoint = categoryEndpoint;
        _loggedInUser = loggedInUser;
    }

    ////////////////////////////////// Properties ///////////////////////

    private string _addCategoryBox = string.Empty;
    public string AddCategoryBox
    {
        get { return _addCategoryBox; }
        set
        {
            _addCategoryBox = value;
            NotifyOfPropertyChange(() => AddCategoryBox);
        }
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

    private Category _selectedCategory;
    public Category SelectedCategory
    {
        get { return _selectedCategory; }
        set { _selectedCategory = value;
            NotifyOfPropertyChange(() => SelectedCategory);
            NotifyOfPropertyChange(() => CanDeleteCategory);
        }
    }

    public bool CanDeleteCategory
    {
        get => SelectedCategory != null;
    }


    ////////////////////////////////// Methods ///////////////////////


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

    public async Task AddCategory()
    {
        if(_addCategoryBox.Length < 1)
        {
            MessageBox.Show("Please enter a category", "Enter Category", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        await _categoryEndpoint.AddCategory(new Category { Name = AddCategoryBox, UserId = _loggedInUser.UserId });
        AddCategoryBox = string.Empty;
        await LoadCategories();
    }

    public async Task DeleteCategory()
    {
        try
        {
            await _categoryEndpoint.DeleteCategory(SelectedCategory.CategoryId);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        await LoadCategories();
    }
}

/*
MIT License

Copyright(c) 2020 Kyle Givler
https://github.com/JoyfulReaper

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using Microsoft.Extensions.DependencyInjection;
using System;
using TimeTrackerLibrary.Data;
using TimeTrackerLibrary.DataAccess;
using TimeTrackerLibrary.Interfaces;
using TimeTrackerLibrary.Services;
using TimeTrackerUI;

namespace TimeTrackerLibrary
{
    public class ContainerBuilder
    {
        public IServiceProvider Build()
        {
            var container = new ServiceCollection();

            IConfig config = new Config();
            config.Initialize();
            var db = config.DBType;

            if (db == DatabaseType.MSSQL)
            {
                container.AddSingleton<ISubcategoryData, SubcategoryData>()
                    .AddSingleton<ICategoryData, CategoryData>()
                    .AddSingleton<IEntryData, EntryData>()
                    .AddSingleton<IProjectData, ProjectData>()
                    .AddSingleton<IDataAccess, SqlDb>();
            }

            if(db == DatabaseType.SQLite)
            {
                container.AddSingleton<IDataAccess, SQLiteDb>()
                    .AddSingleton<ICategoryData, SQLiteCategoryData>()
                    .AddSingleton<ISubcategoryData, SQLiteSubcategoryData>()
                    .AddSingleton<IProjectData, SQLiteProjectData>()
                    .AddSingleton<IEntryData, SQLiteEntryData>();
            }

            container.AddSingleton(_ => container)
                    .AddSingleton<INavigationService, NavigationService>()
                    .AddTransient<frmMain>()
                    .AddTransient<frmAddEntry>()
                    .AddTransient<frmEditCategory>()
                    .AddTransient<frmEditProject>()
                    .AddTransient<frmEditEntry>()
                    .AddSingleton(_ => config)
                    .AddSingleton<IEntryService, EntryService>()
                    .AddSingleton<ICategoryService, CategoryService>()
                    .AddSingleton<ISubcategoryService, SubcategoryService>()
                    .AddSingleton<IProjectService, ProjectService>();

            return container.BuildServiceProvider();
        }
    }
}

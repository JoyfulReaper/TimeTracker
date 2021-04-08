/*
MIT License

Copyright(c) 2021 Kyle Givler
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrackerLibrary.Data;
using TimeTrackerLibrary.Models;
using Xunit;

namespace TimeTrackerTests.Data
{
    public class SQLiteEntryDataTests : DBTest
    {
        private readonly SQLiteEntryData entryData;
        private CategoryModel category;
        private SubcategoryModel subcategory;
        private ProjectModel project;

        public SQLiteEntryDataTests() : base ("EntryTests.db")
        {
            entryData = new SQLiteEntryData(config.Connection);
        }

        [Fact]
        public async Task Test_CreateEntry()
        {
            EntryModel entry = new EntryModel()
            {
                Project = project,
                ProjectId = project.Id,
                Date = DateTimeOffset.Now,
                HoursSpent = 2,
                Notes = "Test Create"
            };

            var id = await entryData.CreateEntry(entry);
            Assert.True(id > 0);

            var dbEntry = await entryData.LoadEntry(id);
            Assert.NotNull(dbEntry);
            Assert.Equal(id, dbEntry.Id);
            Assert.Equal(project.Id, dbEntry.ProjectId);
            Assert.Equal("Test Create", dbEntry.Notes);
            Assert.Equal(2, dbEntry.HoursSpent);
        }

        [Fact]
        public async Task Test_LoadEntry()
        {
            var dbEntry = await entryData.LoadEntry(1);

            Assert.NotNull(dbEntry);
            Assert.Equal(1, dbEntry.Id);
            Assert.Equal(project.Id, dbEntry.ProjectId);
            Assert.Equal("Test Notes", dbEntry.Notes);
            Assert.Equal(1, dbEntry.HoursSpent);
        }

        [Fact]
        public async Task Test_LoadAllEntries()
        {
            EntryModel entry = new EntryModel()
            {
                Project = project,
                ProjectId = project.Id,
                Date = DateTimeOffset.Now,
                HoursSpent = 3,
                Notes = "Test LoadAll"
            };

            int id = await entryData.CreateEntry(entry);

            var entries = await entryData.LoadAllEntries();
            var dbEntry = entries.Where(x => x.Id == id).FirstOrDefault();
            Assert.NotNull(dbEntry);
            Assert.Equal(id, dbEntry.Id);
            Assert.Equal(project.Id, dbEntry.ProjectId);
            Assert.Equal("Test LoadAll", dbEntry.Notes);
            Assert.Equal(3, dbEntry.HoursSpent);
        }

        [Fact]
        public async Task Test_LoadEntriesByProject()
        {
            ProjectModel model = new ProjectModel()
            {
                Name = "Test Project Entries",
                Category = category,
                CategoryId = category.Id,
                Subcategory = subcategory,
                SubcategoryId = subcategory.Id
            };

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("insert into Project(Name, CategoryId, SubcategoryId) ");
            sqlBuilder.Append("values (@Name, @CategoryId, @SubcategoryId); ");
            sqlBuilder.Append("select last_insert_rowid();");

            var queryResult = await config.Connection.QueryRawSQL<Int64, dynamic>(sqlBuilder.ToString(), project);
            model.Id = (int)queryResult.FirstOrDefault();

            EntryModel entry = new EntryModel()
            {
                Project = model,
                ProjectId = model.Id,
                Date = DateTimeOffset.Now,
                HoursSpent = 5,
                Notes = "Test Load By Project"
            };

            EntryModel entry2 = new EntryModel()
            {
                Project = model,
                ProjectId = model.Id,
                Date = DateTimeOffset.Now,
                HoursSpent = 2,
                Notes = "Test Load By Project2"
            };

            await entryData.CreateEntry(entry);
            await entryData.CreateEntry(entry2);

            var entries = await entryData.LoadEntriesByProject(model);
            Assert.NotNull(entries);
            Assert.Equal(2, entries.Count);

            var target = entries.Where(x => x.Id == entry2.Id).FirstOrDefault();
            Assert.NotNull(target);
            Assert.Equal(entry2.Id, target.Id);
            Assert.Equal(model.Id, target.ProjectId);
            Assert.Equal("Test Load By Project2", target.Notes);
            Assert.Equal(2, target.HoursSpent);
        }

        [Fact]
        public async Task Test_RemoveEntry()
        {
            EntryModel entry = new EntryModel()
            {
                Project = project,
                ProjectId = project.Id,
                Date = DateTimeOffset.Now,
                HoursSpent = 5,
                Notes = "Remove Entry"
            };

            var id = await entryData.CreateEntry(entry);
            await entryData.RemoveEntry(entry);
            var dbEntry = await entryData.LoadEntry(id);
            Assert.Null(dbEntry);
        }

        [Fact]
        public async Task Test_RemoveEntryByProject()
        {
            ProjectModel projectModel = new ProjectModel()
            {
                Name = "Remove Project Entries",
                Category = category,
                CategoryId = category.Id,
                Subcategory = subcategory,
                SubcategoryId = subcategory.Id
            };

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("insert into Project(Name, CategoryId, SubcategoryId) ");
            sqlBuilder.Append("values (@Name, @CategoryId, @SubcategoryId); ");
            sqlBuilder.Append("select last_insert_rowid();");

            var queryResult = await config.Connection.QueryRawSQL<Int64, dynamic>(sqlBuilder.ToString(), project);
            projectModel.Id = (int)queryResult.FirstOrDefault();

            EntryModel entry = new EntryModel()
            {
                Project = projectModel,
                ProjectId = projectModel.Id,
                Date = DateTimeOffset.Now,
                HoursSpent = 4,
                Notes = "Test Remove By Project"
            };

            await entryData.CreateEntry(entry);
            var entries = await entryData.LoadEntriesByProject(projectModel);
            Assert.NotNull(entries);
            Assert.True(entries.Count == 1);

            await entryData.RemoveEntryByProject(projectModel);
            entries = await entryData.LoadEntriesByProject(projectModel);
            Assert.NotNull(entries);
            Assert.True(entries.Count == 0);
        }

        [Fact]
        public async Task Test_UpdateEntry()
        {
            EntryModel entry = new EntryModel()
            {
                Project = project,
                ProjectId = project.Id,
                Date = DateTimeOffset.Now,
                HoursSpent = 9,
                Notes = "Test Update"
            };

            var id = await entryData.CreateEntry(entry);
            Assert.True(id > 0);

            entry.Notes = "Test Updated!";
            await entryData.UpdateEntry(entry);

            var dbEntry = await entryData.LoadEntry(id);
            Assert.NotNull(dbEntry);
            Assert.Equal(id, dbEntry.Id);
            Assert.Equal(project.Id, dbEntry.ProjectId);
            Assert.Equal("Test Updated!", dbEntry.Notes);
            Assert.Equal(9, dbEntry.HoursSpent);
        }

        protected override async void Seed()
        {
            // Add test category
            category = new CategoryModel()
            {
                Name = "TestCat"
            };

            string sql = "insert into Category (Name) values (@Name); select last_insert_rowid();";
            var sqlResult = await config.Connection.QueryRawSQL<Int64, dynamic>(sql, category);
            int id = (int)sqlResult.First();
            category.Id = id;

            // Add test subcategory
            subcategory = new SubcategoryModel()
            {
                Name = "TestSubcat",
                Category = category,
                CategoryId = category.Id
            };

            sql = "insert into Subcategory (Name, CategoryId) values (@Name, @CategoryId); select last_insert_rowid();";
            sqlResult = await config.Connection.QueryRawSQL<Int64, dynamic>(sql, subcategory);
            subcategory.Id = (int)sqlResult.FirstOrDefault();

            // Add test project
            project = new ProjectModel()
            {
                Name = "Test Project",
                Category = category,
                CategoryId = category.Id,
                Subcategory = subcategory,
                SubcategoryId = subcategory.Id
            };

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("insert into Project(Name, CategoryId, SubcategoryId) ");
            sqlBuilder.Append("values (@Name, @CategoryId, @SubcategoryId); ");
            sqlBuilder.Append("select last_insert_rowid();");

            var queryResult = await config.Connection.QueryRawSQL<Int64, dynamic>(sqlBuilder.ToString(), project);
            project.Id = (int)queryResult.FirstOrDefault();

            //Add test entry
            EntryModel entry = new EntryModel()
            {
                Project = project,
                ProjectId = project.Id,
                Date = DateTimeOffset.Now,
                HoursSpent = 1,
                Notes = "Test Notes"
            };

            sqlBuilder = new StringBuilder("insert into Entry (ProjectId, HoursSpent, Date, Notes) ");
            sqlBuilder.Append("values (@ProjectId, @HoursSpent, @Date, @Notes); ");
            sqlBuilder.Append("select last_insert_rowid();");

            queryResult = await config.Connection.QueryRawSQL<Int64, dynamic>(sqlBuilder.ToString(), entry);
        }
    }
}

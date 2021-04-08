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
    public class SQLiteProjectDataTests : DBTest
    {
        private readonly SQLiteProjectData projectData;
        private ProjectModel project;
        private CategoryModel category;
        private SubcategoryModel subcategory;

        public SQLiteProjectDataTests() : base("ProjectTests.db")
        {
            projectData = new SQLiteProjectData(config.Connection);
        }

        [Fact]
        public async Task Test_AddProject()
        {
            ProjectModel project = new ProjectModel()
            {
                Name = "Add Project",
                Category = category,
                CategoryId = category.Id,
                Subcategory = subcategory,
                SubcategoryId = subcategory.Id
            };

            int id = await projectData.AddProject(project);
            Assert.True(id > 0);

            var dbProject = await projectData.LoadProject(id);
            Assert.NotNull(dbProject);
            Assert.Equal("Add Project", dbProject.Name);
            Assert.Equal(id, dbProject.Id);
            Assert.Equal(category.Id, dbProject.CategoryId);
            Assert.Equal(subcategory.Id, dbProject.SubcategoryId);
        }

        [Fact]
        public async Task Test_RemoveProject()
        {
            ProjectModel project = new ProjectModel()
            {
                Name = "Remove Project",
                Category = category,
                CategoryId = category.Id,
                Subcategory = subcategory,
                SubcategoryId = subcategory.Id
            };

            int id = await projectData.AddProject(project);
            Assert.True(id > 0);

            await projectData.RemoveProject(project);
            var dbProject = await projectData.LoadProject(id);
            Assert.Null(dbProject);
        }

        [Fact]
        public async Task Test_UpdateProject()
        {
            ProjectModel project = new ProjectModel()
            {
                Name = "Update Project",
                Category = category,
                CategoryId = category.Id,
                Subcategory = subcategory,
                SubcategoryId = subcategory.Id
            };

            int id = await projectData.AddProject(project);
            Assert.True(id > 0);

            project.Name = "Changed Project";
            await projectData.UpdateProject(project);

            var dbProject = await projectData.LoadProject(id);
            Assert.NotNull(dbProject);
            Assert.Equal("Changed Project", dbProject.Name);
            Assert.Equal(id, dbProject.Id);
            Assert.Equal(category.Id, dbProject.CategoryId);
            Assert.Equal(subcategory.Id, dbProject.SubcategoryId);
        }

        [Fact]
        public async Task Test_LoadProject()
        {
            var dbProject = await projectData.LoadProject(1);
            Assert.NotNull(dbProject);
            Assert.Equal("Test Project", dbProject.Name);
            Assert.Equal(1, dbProject.Id);
            Assert.Equal(category.Id, dbProject.CategoryId);
            Assert.Equal(subcategory.Id, dbProject.SubcategoryId);
        }

        [Fact]
        public async Task Test_LoadAllProject()
        {
            ProjectModel project = new ProjectModel()
            {
                Name = "LoadAll Project",
                Category = category,
                CategoryId = category.Id,
                Subcategory = subcategory,
                SubcategoryId = subcategory.Id
            };

            int id = await projectData.AddProject(project);
            var allProject = await projectData.LoadAllProjects();
            var target = allProject.Where(x => x.Id == id).First();
            Assert.NotNull(target);
            Assert.Equal("LoadAll Project", target.Name);
            Assert.Equal(id, target.Id);
            Assert.Equal(category.Id, target.CategoryId);
            Assert.Equal(subcategory.Id, target.SubcategoryId);
        }

        [Fact]
        public async Task Test_ProjectsByCategory()
        {
            ProjectModel project = new ProjectModel()
            {
                Name = "ByCat Project",
                Category = category,
                CategoryId = category.Id,
                Subcategory = subcategory,
                SubcategoryId = subcategory.Id
            };
            int id = await projectData.AddProject(project);

            var cats = await projectData.LoadProjectsByCategory(category);
            Assert.True(cats.TrueForAll(x => x.CategoryId == category.Id));
        }

        [Fact]
        public async Task Test_ProjectsBySubcategory()
        {
            ProjectModel project = new ProjectModel()
            {
                Name = "BySubCat Project",
                Category = category,
                CategoryId = category.Id,
                Subcategory = subcategory,
                SubcategoryId = subcategory.Id
            };
            int id = await projectData.AddProject(project);

            var subs = await projectData.LoadProjectsBySubCategory(subcategory);

            Assert.True(subs.TrueForAll(x => x.SubcategoryId == subcategory.Id));
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
            ProjectModel project = new ProjectModel()
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

            var queryResult = await config.Connection.QueryRawSQL<Int64,dynamic>(sqlBuilder.ToString(), project);
            project.Id = (int)queryResult.FirstOrDefault();
        }
    }
}

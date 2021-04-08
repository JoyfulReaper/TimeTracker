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

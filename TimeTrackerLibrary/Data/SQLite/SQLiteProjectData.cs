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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrackerLibrary.DataAccess;
using TimeTrackerLibrary.Models;

namespace TimeTrackerLibrary.Data
{
    public class SQLiteProjectData : IProjectData
    {
        private readonly IDataAccess dataAccess;

        public SQLiteProjectData(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public async Task<int> AddProject(ProjectModel project)
        {
            if (project.Subcategory != null)
            {
                StringBuilder sql = new StringBuilder("insert into Project (Name, CategoryId, SubcategoryId) ");
                sql.Append("values (@Name, @CategoryId, @SubcategoryId);");
                var sqlResult = await dataAccess.ExecuteRawSQL<dynamic>(sql.ToString(), new
                {
                    Name = project.Name,
                    CategoryId = project.Category.Id,
                    SubcategoryId = project.Subcategory.Id
                });
            }
            else
            {
                StringBuilder sql = new StringBuilder("insert into Project (Name, CategoryId) ");
                sql.Append("values (@Name, @CategoryId);");
                var sqlResult = await dataAccess.ExecuteRawSQL<dynamic>(sql.ToString(), new
                {
                    Name = project.Name,
                    CategoryId = project.Category.Id,
                });
            }

            var queryResult = await dataAccess.QueryRawSQL<Int64, dynamic>("select last_insert_rowid(); ", new { });
            project.Id = (int)queryResult.FirstOrDefault();

            return project.Id;
        }

        public async Task<List<ProjectModel>> LoadAllProjects()
        {
            string sql = "select [Id], [Name], [CategoryId], [SubcategoryId] from Project;";

            var projects = await dataAccess.QueryRawSQL<ProjectModel, dynamic>(sql, new { });
            await RehydrateObjects(projects);

            return projects;
        }

        public async Task<List<ProjectModel>> LoadProjectsByCategory(CategoryModel category)
        {
            StringBuilder sql = new StringBuilder("select [Id], [Name], [CategoryId], [SubcategoryId] from Project ");
            sql.Append("where CategoryId = @CategoryId;");

            var projects = await dataAccess.QueryRawSQL<ProjectModel, dynamic>(sql.ToString(), new { CategoryId = category.Id});
            await RehydrateObjects(projects);

            return projects;
        }

        public async Task<List<ProjectModel>> LoadProjectsBySubCategory(SubcategoryModel subcategory)
        {
            StringBuilder sql = new StringBuilder("select [Id], [Name], [CategoryId], [SubcategoryId] from Project ");
            sql.Append("where SubcategoryId = @SubcategoryId;");

            var projects = await dataAccess.QueryRawSQL<ProjectModel, dynamic>(sql.ToString(), new { SubcategoryId = subcategory.Id });
            await RehydrateObjects(projects);

            return projects;
        }

        public Task RemoveProject(ProjectModel project)
        {
            string sql = "delete from Project where Id = @Id";

            return dataAccess.ExecuteRawSQL(sql, new { Id = project.Id });
        }

        public Task UpdateProject(ProjectModel project)
        {
            StringBuilder sql = new StringBuilder("update Project ");
            sql.Append("set Name = @Name, CategoryId = @CategoryId, SubcategoryId = @SubcategoryId ");
            sql.Append("where Id = @Id;");

            return dataAccess.ExecuteRawSQL(sql.ToString(), new
            {
                Name = project.Name,
                CategoryId = project.Category.Id,
                SubcategoryId = project.Subcategory?.Id,
                Id = project.Id
            });
        }

        private async Task RehydrateObjects(List<ProjectModel> projects)
        {
            //TODO Look into a better way to do this with Dapper
            foreach (var project in projects)
            {
                var category = await dataAccess.QueryRawSQL<CategoryModel, dynamic>("select * from Category where Id = @Id", new { Id = project.CategoryId });
                var subcategory = await dataAccess.QueryRawSQL<SubcategoryModel, dynamic>("select * from Subcategory where Id = @Id", new { Id = project.SubcategoryId });

                project.Category = category.First();
                project.Subcategory = subcategory.FirstOrDefault();
            }
        }
    }
}

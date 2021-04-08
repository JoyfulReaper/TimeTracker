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

using Dapper;

using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using TimeTrackerLibrary.DataAccess;
using TimeTrackerLibrary.Models;

namespace TimeTrackerLibrary.Data
{
    public class ProjectData : IProjectData
    {
        private readonly IDataAccess dataAccess;

        public ProjectData(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public async Task<int> AddProject(ProjectModel project)
        {
            DynamicParameters p = new DynamicParameters();

            p.Add("Name", project.Name);
            p.Add("CategoryId", project.CategoryId);
            p.Add("SubcategoryId", project?.SubcategoryId);
            p.Add("Id", 0, DbType.Int32, direction: ParameterDirection.Output);

            await dataAccess.SaveData("dbo.spProject_Insert", p);

            return p.Get<int>("Id");
        }

        public Task UpdateProject(ProjectModel project)
        {
            DynamicParameters p = new DynamicParameters();

            p.Add("Name", project.Name);
            p.Add("CategoryId", project.CategoryId);
            p.Add("SubcategoryId", project.SubcategoryId);
            p.Add("Id", project.Id);

            return dataAccess.SaveData("dbo.spProject_Update", p);
        }

        public Task RemoveProject(ProjectModel project)
        {
            return dataAccess.SaveData("dbo.spProject_Delete", new { Id = project.Id });
        }

        public async Task<List<ProjectModel>> LoadAllProjects()
        {
            var projects = await dataAccess.LoadData<ProjectModel, dynamic>("dbo.spProject_GetAll", new { });

            await RehydrateObjects(projects);

            return projects;
        }

        public async Task<List<ProjectModel>> LoadProjectsByCategory(CategoryModel category)
        {
            var projects = await dataAccess.LoadData<ProjectModel, dynamic>("dbo.spProject_GetByCatId", new { CategoryId = category.Id});

            await RehydrateObjects(projects);

            return projects;
        }

        public async Task<List<ProjectModel>> LoadProjectsBySubCategory(SubcategoryModel subcategory)
        {
            var projects = await dataAccess.LoadData<ProjectModel, dynamic>("dbo.spProject_GetBySubCatId", new { SubcategoryId = subcategory.Id });

            await RehydrateObjects(projects);

            return projects;
        }

        private async Task RehydrateObjects(List<ProjectModel> projects)
        {
            //TODO Look into a better way to do this with Dapper
            foreach (var project in projects)
            {
                var category = await dataAccess.LoadData<CategoryModel, dynamic>("spCategory_GetById", new { Id = project.CategoryId });
                var subcategory = await dataAccess.LoadData<SubcategoryModel, dynamic>("spSubcategory_GetById", new { Id = project.SubcategoryId });

                project.Category = category.First();
                project.Subcategory = subcategory.FirstOrDefault();
            }
        }
    }
}

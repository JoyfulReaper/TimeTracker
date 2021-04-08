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

using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackerLibrary.DataAccess;
using TimeTrackerLibrary.Models;

namespace TimeTrackerLibrary.Data
{
    public class EntryData : IEntryData
    {
        private readonly IDataAccess dataAccess;

        public EntryData(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public Task<int> CreateEntry(EntryModel entry)
        {
            DynamicParameters p = new DynamicParameters();

            p.Add("ProjectId", entry.ProjectId);
            p.Add("HoursSpent", entry.HoursSpent);
            p.Add("Date", entry.Date);
            p.Add("Notes", entry.Notes);
            p.Add("Id", 0, DbType.Int32, direction: ParameterDirection.Output);

            return dataAccess.SaveData("dbo.spEntry_Insert", p);
        }

        public Task UpdateEntry(EntryModel entry)
        {
            DynamicParameters p = new DynamicParameters();

            p.Add("HoursSpent", entry.HoursSpent);
            p.Add("Notes", entry.Notes);
            p.Add("Date", entry.Date);

            return dataAccess.SaveData("spEntry_Update", p);
        }

        public async Task<List<EntryModel>> LoadAllEntries()
        {
            var entries = await dataAccess.LoadData<EntryModel, dynamic>("dbo.spEntry_GetAll", new { });

            foreach (var e in entries)
            {
                await RehydrateObjects(e);
            }
            
            return entries;
        }

        public Task<List<EntryModel>> LoadEntriesByCategory(CategoryModel category)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<EntryModel>> LoadEntriesByProject(ProjectModel project)
        {
            var entries = await dataAccess.LoadData<EntryModel, dynamic>("dbo.spEntry_GetByProjectId", new { ProjectId = project.Id });

            foreach (var e in entries)
            {
                await RehydrateObjects(e);
            }

            return entries;
        }

        public Task<List<EntryModel>> LoadEntriesBySubcategory(SubcategoryModel subcategory)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveEntry(EntryModel entry)
        {
            return dataAccess.SaveData("spEntry_Delete", new { Id = entry.Id });
        }

        public Task RemoveEntryByProject(ProjectModel project)
        {
            return dataAccess.SaveData("spEntry_DeleteByProject", new { ProjectId = project.Id});
        }

        private async Task RehydrateObjects(EntryModel e)
        {
            // TODO find a better way to do this with Dapper
            var p = await dataAccess.QueryRawSQL<ProjectModel, dynamic>("SELECT * FROM Project WHERE ID = @Id", new { Id = e.ProjectId });
            e.Project = p.First();
            var c = await dataAccess.QueryRawSQL<CategoryModel, dynamic>("SELECT * FROM Category WHERE ID = @Id", new { Id = e.Project.CategoryId });
            e.Project.Category = c.First();
            var s = await dataAccess.QueryRawSQL<SubcategoryModel, dynamic>("SELECT * FROM Subcategory WHERE ID = @Id", new { Id = e.Project.SubcategoryId });
            e.Project.Subcategory = s.FirstOrDefault();
        }
    }
}

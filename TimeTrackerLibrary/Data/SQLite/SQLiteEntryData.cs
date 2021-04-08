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
    public class SQLiteEntryData : IEntryData
    {
        private readonly IDataAccess dataAccess;

        public SQLiteEntryData(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public async Task<int> CreateEntry(EntryModel entry)
        {
            StringBuilder sql = new StringBuilder("insert into Entry (ProjectId, HoursSpent, Date, Notes) ");
            sql.Append("values (@ProjectId, @HoursSpent, @Date, @Notes);");

            var sqlResult = await dataAccess.ExecuteRawSQL<dynamic>(sql.ToString(), new
            {
                ProjectId = entry.Project.Id,
                HoursSpent = entry.HoursSpent,
                Date = entry.Date,
                Notes = entry.Notes
            });
            var queryResult = await dataAccess.QueryRawSQL<Int64, dynamic>("select last_insert_rowid();", new { });
            entry.Id = (int)queryResult.FirstOrDefault();

            return entry.Id;
        }

        public async Task UpdateEntry(EntryModel entry)
        {
            string sql = "update Entry set HoursSpent = @HoursSpent, Notes = @Notes, Date = @Date where Id = @Id";
            await dataAccess.ExecuteRawSQL<dynamic>(sql, entry);
        }

        public async Task<List<EntryModel>> LoadAllEntries()
        {
            string sql = "select [Id], [ProjectId], [HoursSpent], [Date], [Notes] from Entry;";

            var entries = await dataAccess.QueryRawSQL<EntryModel, dynamic>(sql, new { });

            foreach (var e in entries)
            {
                await RehydrateObjects(e);
            }

            return entries;
        }

        public Task<List<EntryModel>> LoadEntriesByCategory(CategoryModel category)
        {
            throw new NotImplementedException();
        }

        public async Task<List<EntryModel>> LoadEntriesByProject(ProjectModel project)
        {
            string sql = "select [Id], [ProjectId], [HoursSpent], [Date], [Notes] from Entry where ProjectId = @ProjectId;";

            var entries = await dataAccess.QueryRawSQL<EntryModel, dynamic>(sql, new { ProjectId = project.Id});

            foreach (var e in entries)
            {
                await RehydrateObjects(e);
            }

            return entries;
        }

        public Task<List<EntryModel>> LoadEntriesBySubcategory(SubcategoryModel subcategory)
        {
            throw new NotImplementedException();
        }

        public Task RemoveEntry(EntryModel entry)
        {
            string sql = "delete from Entry where Id = @Id";

            return dataAccess.ExecuteRawSQL<dynamic>(sql, new { Id = entry.Id });
        }

        public Task RemoveEntryByProject(ProjectModel project)
        {
            string sql = "delete from Entry where ProjectId = @ProjectId";

            return dataAccess.ExecuteRawSQL<dynamic>(sql, new { ProjectId = project.Id });
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

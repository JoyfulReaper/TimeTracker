﻿/*
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

using System.Linq;
using System.Threading.Tasks;
using TimeTrackerLibrary.Data;
using TimeTrackerLibrary.Interfaces;
using TimeTrackerLibrary.Models;

namespace TimeTrackerLibrary.Services
{
    public sealed class EntryService : IEntryService
    {
        private readonly IConfig config;

        public EntryService(IConfig config)
        {
            this.config = config;
        }

        public async Task<double> GetTotalTimeAllEntries()
        {
            var res = await config.Connection.QueryRawSQL<double, dynamic>("select SUM(hoursSpent) from Entry;", new { });
            return res.FirstOrDefault();
        }

        public async Task<double> GetTimeByProject(ProjectModel project)
        {
            var res = await config.Connection.QueryRawSQL<double, dynamic>($"select SUM(hoursSpent) from Entry WHERE ProjectId = {project.Id};", new { });
            return res.FirstOrDefault();
        }

        public async Task<double> GetTimeByCategory(CategoryModel category)
        {
            var sql = $"select SUM(e.hoursSpent) from Entry e inner join Project p on e.ProjectId = p.Id inner join Category c on p.CategoryId = c.Id where p.CategoryId = {category.Id};";

            var res = await config.Connection.QueryRawSQL<double, dynamic>(sql, new { });
            return res.FirstOrDefault();
        }

        public async Task<double> GetTimeBySubcategory(SubcategoryModel subcategory)
        {
            var sql = $"select SUM(e.hoursSpent) from Entry e inner join Project p on e.ProjectId = p.Id inner join Subcategory s on p.SubcategoryId = s.Id where p.SubcategoryId = {subcategory.Id};";

            var res = await config.Connection.QueryRawSQL<double, dynamic>(sql, new { });
            return res.FirstOrDefault();
        }
    }
}

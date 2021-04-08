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
    public class SQLiteSubcategoryData : ISubcategoryData
    {
        private readonly IDataAccess dataAccess;

        public SQLiteSubcategoryData(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public async Task<int> AddSubcategory(SubcategoryModel subcategory)
        {
            string sql = "insert into Subcategory (Name, CategoryId) values (@Name, @CategoryId); select last_insert_rowid();";
            var queryResult = await dataAccess.QueryRawSQL<Int64, dynamic>(sql, subcategory);

            subcategory.Id = (int)queryResult.FirstOrDefault();
            return subcategory.Id;
        }

        public async Task<List<SubcategoryModel>> LoadAllSubcategories(CategoryModel category)
        {
            string sql = "select [Id], [Name], [CategoryId] from Subcategory where CategoryId = @CategoryId;";
            var qureyResult = await dataAccess.QueryRawSQL<SubcategoryModel, dynamic>(sql, new { CategoryId = category.Id });

            foreach (var q in qureyResult)
            {
                q.Category = category;
            }

            return qureyResult;
        }

        public async Task<SubcategoryModel> LoadSubcategory(int id)
        {
            string sql = "select [Id], [Name], [CategoryId] from Subcategory where Id = @Id;";
            var qureyResult = await dataAccess.QueryRawSQL<SubcategoryModel, dynamic>(sql, new { Id = id });
            var output = qureyResult.FirstOrDefault();

            if(output == null)
            {
                return null;
            }

            sql = "select [Id], [Name] from Category where Id = @id;";
            var sqlResult = await dataAccess.QueryRawSQL<CategoryModel, dynamic>(sql, new { Id = output.CategoryId });
            output.Category = sqlResult.FirstOrDefault();

            return output;
        }

        public Task RemoveSubcategory(SubcategoryModel subcategory)
        {
            string sql = "delete from Subcategory where Id = @Id;";

            return dataAccess.ExecuteRawSQL<dynamic>(sql, new { Id = subcategory.Id });
        }

        public Task UpdateSubcategory(SubcategoryModel subcategory)
        {
            string sql = "update Subcategory set Name = @Name where Id = @Id";

            return dataAccess.ExecuteRawSQL<dynamic>(sql, new { Id = subcategory.Id, Name = subcategory.Name });
        }
    }
}

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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackerLibrary.DataAccess;
using TimeTrackerLibrary.Models;

namespace TimeTrackerLibrary.Data
{
    public class SQLiteCategoryData : ICategoryData
    {
        private readonly IDataAccess dataAccess;

        public SQLiteCategoryData(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public async Task<int> AddCategory(CategoryModel category)
        {
            string sql = "insert into Category (Name) values (@Name); select last_insert_rowid();";
            var queryResult = await dataAccess.QueryRawSQL<Int64, dynamic>(sql, new { Name = category.Name});
            category.Id = (int)queryResult.FirstOrDefault();

            return category.Id;
        }

        public async Task<CategoryModel> LoadCategory(int id)
        {
            string sql = "select [Id], [Name] from Category where Id = @id;";

            var sqlResult = await dataAccess.QueryRawSQL<CategoryModel, dynamic>(sql, new { Id = id });
            return sqlResult.FirstOrDefault();
        }

        public Task<List<CategoryModel>> LoadAllCategories()
        {
            string sql = "select [Id], [Name] from Category;";

            return dataAccess.QueryRawSQL<CategoryModel, dynamic>(sql, new { });
        }

        public Task RemoveCategory(CategoryModel category)
        {
            string sql = "delete from Category where Id = @Id;";

            return dataAccess.ExecuteRawSQL<dynamic>(sql, new { Id = category.Id });
        }

        public Task UpdateCategory(CategoryModel category)
        {
            string sql = "update Category set Name = @Name where Id = @Id;";

            return dataAccess.ExecuteRawSQL<dynamic>(sql, new { Name = category.Name, Id = category.Id });
        }
    }
}

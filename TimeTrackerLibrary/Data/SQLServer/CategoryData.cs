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
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackerLibrary.DataAccess;
using TimeTrackerLibrary.Models;

namespace TimeTrackerLibrary.Data
{
    public class CategoryData : ICategoryData
    {
        public readonly IDataAccess dataAccess;

        public CategoryData(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        /// <summary>
        /// Add a category
        /// </summary>
        /// <param name="category">The CategoryModel to add to the database</param>
        /// <returns>The Id of the CategoryModel</returns>
        public async Task<int> AddCategory(CategoryModel category)
        {
            DynamicParameters p = new DynamicParameters();

            p.Add("Name", category.Name);
            p.Add("Id", 0, DbType.Int32, direction: ParameterDirection.Output);

            await dataAccess.SaveData("dbo.spCategory_Insert", p);

            return p.Get<int>("Id");
        }

        public Task UpdateCategory(CategoryModel category)
        {
            DynamicParameters p = new DynamicParameters();

            p.Add("Name", category.Name);
            p.Add("Id", category.Id);

            return dataAccess.SaveData("dbo.spCategory_Update", p);
        }

        /// <summary>
        /// Load all Categories from the database
        /// </summary>
        /// <returns>A list of all CategoryModels in the database</returns>
        public Task<List<CategoryModel>> LoadAllCategories()
        {
            return dataAccess.LoadData<CategoryModel, dynamic>("dbo.spCategory_GetAll", new { });
        }

        public async Task<CategoryModel> LoadCategory(int id)
        {
            string sql = "select [Id], [Name] from Category where Id = @id;";

            var sqlResult = await dataAccess.QueryRawSQL<CategoryModel, dynamic>(sql, new { Id = id });
            return sqlResult.FirstOrDefault();
        }

        /// <summary>
        /// Remove category from the database
        /// </summary>
        /// <param name="category">The Category to remove</param>
        public Task RemoveCategory(CategoryModel category)
        {
            return dataAccess.SaveData("dbo.spCategory_Delete", new { Id = category.Id });
        }
    }
}

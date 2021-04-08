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
using TimeTrackerLibrary.Data;
using TimeTrackerLibrary.Models;
using Xunit;

namespace TimeTrackerTests.Data
{
    public class SQLiteCategoryDataTests : DBTest
    {
        private readonly SQLiteCategoryData categoryData;

        public SQLiteCategoryDataTests() : base("CatTests.db")
        {
            categoryData = new SQLiteCategoryData(config.Connection);
        }

        [Fact]
        public async Task Test_AddCategory()
        {
            CategoryModel cat = new CategoryModel() { Name = "AddCategory" };
            var id = await categoryData.AddCategory(cat);

            var dbCat = await categoryData.LoadCategory(id);
            Assert.NotNull(dbCat);
            Assert.True(cat.Id > 0);
            Assert.Equal("AddCategory", dbCat.Name);
            Assert.Equal(id, dbCat.Id);
        }

        [Fact]
        public async Task Test_UpdateCategory()
        {
            CategoryModel cat = new CategoryModel() { Name = "UpdateCategory" };
            var id = await categoryData.AddCategory(cat);

            var dbCat = await categoryData.LoadCategory(id);
            Assert.NotNull(dbCat);
            Assert.True(cat.Id > 0);
            Assert.Equal("UpdateCategory", dbCat.Name);
            Assert.Equal(id, dbCat.Id);

            dbCat.Name = "TotalyUpdatedMan";
            await categoryData.UpdateCategory(dbCat);

            dbCat = await categoryData.LoadCategory(id);
            Assert.NotNull(dbCat);
            Assert.True(cat.Id > 0);
            Assert.Equal("TotalyUpdatedMan", dbCat.Name);
            Assert.Equal(id, dbCat.Id);
        }

        [Fact]
        public async Task Test_LoadCategory()
        {
            var dbCat = await categoryData.LoadCategory(1);
            Assert.NotNull(dbCat);
            Assert.Equal("TestCat", dbCat.Name);
            Assert.Equal(1, dbCat.Id);
        }

        [Fact]
        public async Task Test_LoadAllCategories()
        {
            CategoryModel cat = new CategoryModel() { Name = "LoadAllCategory" };
            var id = await categoryData.AddCategory(cat);

            var allCats = await categoryData.LoadAllCategories();
            Assert.NotNull(allCats);
            Assert.True(allCats.Count() > 0);


            var dbCat = allCats.Where(x => x.Id == id).First();
            Assert.Equal("LoadAllCategory", dbCat.Name);
            Assert.Equal(id, dbCat.Id);
        }

        [Fact]
        public async Task Test_RemoveCategory()
        {
            CategoryModel cat = new CategoryModel() { Name = "RemoveCategory" };
            var id = await categoryData.AddCategory(cat);

            await categoryData.RemoveCategory(cat);
            var dbCat = await categoryData.LoadCategory(id);
            Assert.Null(dbCat);
        }

        protected override async void Seed()
        {
            string sql = "insert into Category (Name) values (@Name); select last_insert_rowid();";

            var sqlResult = await config.Connection.QueryRawSQL<Int64, dynamic>(sql, new { Name = "TestCat" });
            int id = (int)sqlResult.First();
        }
    }
}

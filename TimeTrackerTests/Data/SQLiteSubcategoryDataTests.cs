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
    public class SQLiteSubcategoryDataTests : DBTest
    {
        private CategoryModel category;
        private readonly SQLiteSubcategoryData subcategoryData;

        public SQLiteSubcategoryDataTests() : base("subCatTests.db")
        {
            subcategoryData = new SQLiteSubcategoryData(config.Connection);
        }

        [Fact]
        public async Task Test_AddSubcategory()
        {
            SubcategoryModel sub = new SubcategoryModel()
            {
                Name = "AddSubcategoryTest",
                Category = category,
                CategoryId = category.Id
            };

            var id = await subcategoryData.AddSubcategory(sub);
            Assert.True(id > 0);

            var dbSub = await subcategoryData.LoadSubcategory(id);
            Assert.NotNull(dbSub);
            Assert.Equal("AddSubcategoryTest", dbSub.Name);
            Assert.Equal(id, dbSub.Id);
            Assert.Equal(category.Id, dbSub.CategoryId);
        }

        [Fact]
        public async Task Test_LoadAllSubcategories()
        {
            SubcategoryModel sub = new SubcategoryModel()
            {
                Name = "LoadAllSubcategoryTest",
                Category = category,
                CategoryId = category.Id
            };

            var id = await subcategoryData.AddSubcategory(sub);
            Assert.True(id > 0);

            var allSub = await subcategoryData.LoadAllSubcategories(category);
            Assert.NotNull(allSub);
            Assert.True(allSub.Count > 1);

            var dbSub = allSub.Where(x => x.Id == id).FirstOrDefault();
            Assert.NotNull(dbSub);
            Assert.Equal("LoadAllSubcategoryTest", dbSub.Name);
            Assert.Equal(id, dbSub.Id);
            Assert.Equal(category.Id, dbSub.CategoryId);
        }

        [Fact]
        public async Task Test_LoadSubcategory()
        {
            var dbSub = await subcategoryData.LoadSubcategory(1);
            Assert.NotNull(dbSub);
            Assert.Equal("SubcatTest", dbSub.Name);
            Assert.Equal(1, dbSub.Id);
            Assert.Equal(category.Id, dbSub.CategoryId);
        }

        [Fact]
        public async Task Test_RemoveSubcategory()
        {
            SubcategoryModel sub = new SubcategoryModel()
            {
                Name = "RemoveSubcategoryTest",
                Category = category,
                CategoryId = category.Id
            };

            var id = await subcategoryData.AddSubcategory(sub);
            Assert.True(id > 0);

            await subcategoryData.RemoveSubcategory(sub);

            var dbSub = await subcategoryData.LoadSubcategory(id);
            Assert.Null(dbSub);
        }

        [Fact]
        public async Task Test_UpdateSubcategory()
        {
            SubcategoryModel sub = new SubcategoryModel()
            {
                Name = "UpdateSubcategoryTest",
                Category = category,
                CategoryId = category.Id
            };

            var id = await subcategoryData.AddSubcategory(sub);
            Assert.True(id > 0);

            sub.Name = "ChangedSubcategoryTest";
            await subcategoryData.UpdateSubcategory(sub);

            var dbSub = await subcategoryData.LoadSubcategory(id);
            Assert.NotNull(dbSub);
            Assert.Equal("ChangedSubcategoryTest", dbSub.Name);
            Assert.Equal(id, dbSub.Id);
            Assert.Equal(category.Id, dbSub.CategoryId);
        }

        protected override async void Seed()
        {
            category = new CategoryModel()
            {
                Name = "TestCat"
            };

            string sql = "insert into Category (Name) values (@Name); select last_insert_rowid();";

            var sqlResult = await config.Connection.QueryRawSQL<Int64, dynamic>(sql, category);
            int id = (int)sqlResult.First();
            category.Id = id;

            SubcategoryModel subcat = new SubcategoryModel()
            {
                Name = "SubcatTest",
                Category = category,
                CategoryId = category.Id
            };

            sql = "insert into Subcategory (Name, CategoryId) values (@Name, @CategoryId);";
            sqlResult = await config.Connection.QueryRawSQL<Int64, dynamic>(sql, subcat);
        }
    }
}

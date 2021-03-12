using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackerLibrary.Data;
using TimeTrackerLibrary.DataAccess;
using TimeTrackerLibrary.Models;
using Xunit;

namespace TimeTrackerTests
{
    public class CategoryDataTests
    {
        [Fact]
        public async Task LoadAllCategories_ValidCall()
        {
            var mockCategoryData = new Mock<IDataAccess>();
            mockCategoryData.Setup(x => x.QueryRawSQL<CategoryModel, dynamic> ("select [Id], [Name] from Category;", It.IsAny<object>() )).ReturnsAsync(GetSampleCategories());

            var cls = new SQLiteCategoryData(mockCategoryData.Object);
            var test = await cls.LoadAllCategories();

            Assert.True(test != null);
        }

        private List<CategoryModel> GetSampleCategories()
        {
            List<CategoryModel> output = new List<CategoryModel>
            {
                new CategoryModel
                {
                    Id = 1,
                    Name = "Test Cat1"
                },
                new CategoryModel
                {
                    Id = 2,
                    Name = "Test Cat2"
                }
            };

            return output;
        }
    }
}

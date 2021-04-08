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

using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackerLibrary.Models;

namespace TimeTrackerLibrary.Data
{
    public interface ICategoryData
    {
        /// <summary>
        /// Add a category
        /// </summary>
        /// <param name="category">The CategoryModel to add to the database</param>
        /// <returns>The Id of the CategoryModel</returns>
        Task<int> AddCategory(CategoryModel category);

        /// <summary>
        /// Update a Category
        /// </summary>
        /// <param name="category">The category to update</param>
        Task UpdateCategory(CategoryModel category);

        Task<CategoryModel> LoadCategory(int id);

        /// <summary>
        /// Load all Categories from the database
        /// </summary>
        /// <returns>A list of all CategoryModels in the database</returns>
        Task<List<CategoryModel>> LoadAllCategories();

        /// <summary>
        /// Remove category from the database
        /// </summary>
        /// <param name="category">The Category to remove</param>
        Task RemoveCategory(CategoryModel category);
    }
}
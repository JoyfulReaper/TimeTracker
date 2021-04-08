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
    public interface ISubcategoryData
    {
        /// <summary>
        /// Add a subcategory to the database
        /// </summary>
        /// <param name="subcategory">The subcategory to add</param>
        /// <returns>The id of the subcategory</returns>
        Task<int> AddSubcategory(SubcategoryModel subcategory);

        /// <summary>
        /// Update an subcategory
        /// </summary>
        /// <param name="subcategory">The subcategory to update</param>
        Task UpdateSubcategory(SubcategoryModel subcategory);

        Task<SubcategoryModel> LoadSubcategory(int id);

        /// <summary>
        /// Load subcategories associated with a category
        /// </summary>
        /// <param name="category">The category for which to load subcategories</param>
        /// <returns>A list of SubcategoryModels associates with a category</returns>
        Task<List<SubcategoryModel>> LoadAllSubcategories(CategoryModel category);

        /// <summary>
        /// Remove a subcategory
        /// </summary>
        /// <param name="subcategory">The subcategory to remove</param>
        Task RemoveSubcategory(SubcategoryModel subcategory);
    }
}
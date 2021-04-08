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
    public interface IEntryData
    {
        /// <summary>
        /// Create an Entry
        /// </summary>
        /// <param name="entry">The Entry to save in the database</param>
        /// <returns></returns>
        Task<int> CreateEntry(EntryModel entry);

        Task<EntryModel> LoadEntry(int id);

        /// <summary>
        /// Load all Entries from the database
        /// </summary>
        /// <returns>List of all Entries</returns>
        Task<List<EntryModel>> LoadAllEntries();

        /// <summary>
        /// Load all entries by category
        /// </summary>
        /// <param name="category">The category for which to load entries</param>
        /// <returns>List of entires in the given category</returns>
        Task<List<EntryModel>> LoadEntriesByCategory(CategoryModel category);

        /// <summary>
        /// Load all entries by subcategory
        /// </summary>
        /// <param name="subcategory">The category for which to load entries</param>
        /// <returns>List of entires in the given subcategory</returns>
        Task<List<EntryModel>> LoadEntriesBySubcategory(SubcategoryModel subcategory);

        /// <summary>
        /// Load all entries by Project
        /// </summary>
        /// <param name="subcategory">The project for which to load entries</param>
        /// <returns>List of entires in the given project</returns>
        Task<List<EntryModel>> LoadEntriesByProject(ProjectModel project);

        /// <summary>
        /// Remove an entry
        /// </summary>
        /// <param name="entry">The Entry to remove</param>
        Task RemoveEntry(EntryModel entry);

        /// <summary>
        /// Delete all entries for a given project
        /// </summary>
        /// <param name="project">The project for which to delete entries</param>
        Task RemoveEntryByProject(ProjectModel project);

        /// <summary>
        /// Edit an entry
        /// </summary>
        /// <param name="entry">The Entry to edit</param>
        Task UpdateEntry(EntryModel entry);
    }
}
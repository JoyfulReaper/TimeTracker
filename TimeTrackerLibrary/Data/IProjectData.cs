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
    public interface IProjectData
    {
        /// <summary>
        /// Save a project to the database
        /// </summary>
        /// <param name="project">The project to save</param>
        /// <returns>The id of the project</returns>
        Task<int> AddProject(ProjectModel project);

        /// <summary>
        /// Remove a Project
        /// </summary>
        /// <param name="project">The project to remove</param>
        Task RemoveProject(ProjectModel project);

        /// <summary>
        /// Update a Project
        /// </summary>
        /// <param name="project">The Project to update</param>
        Task UpdateProject(ProjectModel project);

        Task<ProjectModel> LoadProject(int id);

        /// <summary>
        /// Load all projects from the database
        /// </summary>
        /// <returns>A list of all the ProjectModels saved to the database</returns>
        Task<List<ProjectModel>> LoadAllProjects();

        /// <summary>
        /// Load all the projects in the given category
        /// </summary>
        /// <param name="category">The category for which to load projects</param>
        Task<List<ProjectModel>> LoadProjectsByCategory(CategoryModel category);

        /// <summary>
        /// Load all the projects in the given subcategory
        /// </summary>
        /// <param name="subcategory">The subcategory for which to load projects</param>
        Task<List<ProjectModel>> LoadProjectsBySubCategory(SubcategoryModel subcategory);
    }
}
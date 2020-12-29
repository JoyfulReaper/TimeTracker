﻿/*
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
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTrackerLibrary.DataAccess
{
    public class SqlDb : IDataAccess
    {
        /// <summary>
        /// Load data from the database
        /// </summary>
        /// <typeparam name="T">Type of the data to retreive</typeparam>
        /// <param name="storedProcedure">The store procedure to execute</param>
        /// <param name="parameters">Paramaters for the stored procedure</param>
        /// <returns>A list of type T</returns>
        public async Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString()))
            {
                var rows = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

                return rows.ToList();
            }
        }

        /// <summary>
        /// Save data to the database
        /// </summary>
        /// <param name="storedProcedure">The stored procedure to execute</param>
        /// <param name="parameters">The paremeters for the store procedure</param>
        /// <returns>The number of rows affected</returns>
        public async Task<int> SaveData<T>(string storedProcedure, T parameters)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString()))
            {
                return await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<List<T>> QueryRawSQL<T>(string sql)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString()))
            {
                var res = await connection.QueryAsync<T>(sql);
                return res.ToList();
            }
        }
    }
}

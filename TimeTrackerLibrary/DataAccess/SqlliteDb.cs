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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackerLibrary.Interfaces;
using TimeTrackerLibrary.Properties;

namespace TimeTrackerLibrary.DataAccess
{
    public class SqlliteDb : IDataAccess
    {
        private readonly IConfig config;

        public SqlliteDb(IConfig config)
        {
            this.config = config;
        }

        public Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters)
        {
            throw new NotImplementedException();
        }


        public Task<int> SaveData<T>(string storedProcedure, T parameters)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> QueryRawSQL<T, U>(string sql, U parameters)
        {
            using (IDbConnection connection = new SQLiteConnection(config.ConnectionString()))
            {
                var res = await connection.QueryAsync<T>(sql, parameters);
                return res.ToList();
            }
        }

        public async Task<int> ExecuteRawSQL<T>(string sql, T parameters)
        {
            using (IDbConnection connection = new SQLiteConnection(config.ConnectionString()))
            {
                var res = await connection.ExecuteAsync(sql, parameters);
                return res;
            }
        }

        public async Task CreateDatabase()
        {
            string sql = Resources.CreateSQLiteDB;

            await ExecuteRawSQL<dynamic>(sql, null);
        }
    }
}

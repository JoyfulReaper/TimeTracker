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

using Microsoft.Extensions.Configuration;
using System;
using TimeTrackerLibrary.DataAccess;

namespace TimeTrackerLibrary
{
    public enum DatabaseType { MSSQL };

    public static class GlobalConfig
    {
        public static IDataAccess Connection { get; private set; }
        public static IConfiguration Configuration { get; private set; }
        public static DatabaseType DBTtype { get; private set; }

        static GlobalConfig()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true).Build();
        }

        public static void Initialize(DatabaseType db)
        {
            if(db == DatabaseType.MSSQL)
            {
                SqlDb sql = new SqlDb();
                Connection = sql;
                DBTtype = db;
                return;
            }

            throw new ArgumentException("Data source not valid", "db");
        }

        public static string ConnectionString()
        {
            if(DBTtype == DatabaseType.MSSQL)
            {
                return Configuration.GetConnectionString("MSSQL");
            }

            throw new InvalidOperationException("DBType is not valid");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrackerApi.Library.DataAccess;
public interface IDataAccess
{
    void CommitTransaction();
    void Dispose();
    string GetConnectionString(string name);
    Task<List<T>> LoadDataAsync<T, U>(string storedProcedure, U parameters, string connectionStringName);
    Task<List<T>> LoadDataInTransactionAsync<T, U>(string storedProcedure, U parameters);
    void RollbackTransaction();
    Task SaveDataAsync<T>(string storedProcedure, T parameters, string connectionStringName);
    Task SaveDataInTransactionAsync<T>(string storedProcedure, T parameters);
    Task<int> SaveDataAndGetIdAsync<T>(string storedProcedure, T parameters, string connectionStringName);
    void StartTransaction(string connectionStringName);
}

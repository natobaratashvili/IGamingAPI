using IGaming.Core.Common;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace IGaming.Core.Database
{
    public interface IDbWrapper
    {
        Task<T> RunAsync<T>(Func<IDbConnection, Task<T>> action, CancellationToken cancellationToken);
        Task<T> RunWithTransactionAsync<T>(Func<IDbConnection, IDbTransaction, Task<T>> action, CancellationToken cancellationToken);
        
    }
}

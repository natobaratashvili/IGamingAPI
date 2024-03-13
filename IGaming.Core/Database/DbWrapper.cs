using IGaming.Core.DatabaseAccessHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.Database
{
    public class DbWrapper : IDbWrapper
    {
        private readonly IDbConnectionProvider db;
        public DbWrapper(IDbConnectionProvider dbConnection)
        {
            db = dbConnection;
        }
      

        public async Task<T> RunAsync<T>(Func<IDbConnection, Task<T>> action, CancellationToken cancellationToken)
        {
            using var conn = await db.CreateConnectionAsync(cancellationToken);
            return await action(conn);

        }

    

        public async Task<T> RunWithTransactionAsync<T>(Func<IDbConnection, IDbTransaction, Task<T>> action, CancellationToken cancellationToken)
        {
            using var conn = await db.CreateConnectionAsync(cancellationToken);
            using var transaction = conn.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                var result = await action(conn, transaction);

                transaction.Commit();

                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}

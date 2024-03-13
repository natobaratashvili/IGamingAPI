using IGaming.Core.DatabaseAccessHelpers;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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

            for(var i = 0; i < 3;i++)
            {
                using var transaction = conn.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    var result = await action(conn, transaction);

                    transaction.Commit();

                    return result;
                }
                catch(MySqlException ex)
                {
                    transaction.Rollback();
                    if (ex.Number == 1213 && i < 2)
                    {
                        
                        await Task.Delay(10, cancellationToken);

                    }  else
                    {
                        throw;
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return default;
          
        }
    }
}

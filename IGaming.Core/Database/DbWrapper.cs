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
    /// <summary>
    /// Wrapper class for database operations.
    /// </summary>
    public class DbWrapper : IDbWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbWrapper"/> class.
        /// </summary>
        /// <param name="dbConnection">The database connection provider.</param>
        private readonly IDbConnectionProvider db;
        public DbWrapper(IDbConnectionProvider dbConnection)
        {
            db = dbConnection;
        }

        /// <summary>
        /// Executes the specified database action asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="action">The database action to execute.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation. The result of the database action.</returns>
        public async Task<T> RunAsync<T>(Func<IDbConnection, Task<T>> action, CancellationToken cancellationToken)
        {
            using var conn = await db.CreateConnectionAsync(cancellationToken);
            return await action(conn);

        }

        /// <summary>
        /// Executes the specified database action within a transaction asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="action">The database action to execute.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation. The result of the database action.</returns>
        /// <remarks>
        /// This method executes the specified database action within a transaction. If the transaction encounters a deadlock (error code 1213),
        /// it retries the operation up to three times with a slight delay between retries to mitigate deadlocks. If the maximum number of retries
        /// is reached, the method throws an exception.
        /// </remarks>
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
                    if (ex.Number == 1213 && i < 2) //deadlock
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

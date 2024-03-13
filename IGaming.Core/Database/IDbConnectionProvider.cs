using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.DatabaseAccessHelpers
{
    /// <summary>
    /// Interface for providing database connections asynchronously.
    /// </summary>
    public interface IDbConnectionProvider
    {
        /// <summary>
        /// Creates a new database connection asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation. The database connection.</returns>
        Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken);

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.DatabaseAccessHelpers
{
    public interface IDbConnectionProvider
    {
        Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken);

    }
}

using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.DatabaseAccessHelpers
{
    public class MySqlConnectionProvider : IDbConnectionProvider
    {
        private readonly string _connectionString;

        public MySqlConnectionProvider(IOptions<DbConfig> dbConfig)
        {
            _connectionString = dbConfig.Value.ConnectionString;
        }

        public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken)
        {
            var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            return connection;
        }
    }
}

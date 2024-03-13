using Dapper;
using IGaming.Core.Bets.Dtos;
using IGaming.Core.Bets.Repositories.Interfaces;
using IGaming.Core.DatabaseAccessHelpers;
using MySql.Data.MySqlClient;
using System.Data;

namespace IGaming.Core.Bets.Repositories.Implementation
{
    public class BetsRepository : IBetsRepository
    {
        public async Task InsertAsync(IDbConnection con, BetDto bet, IDbTransaction? transaction = null)
        {
            await con.ExecuteAsync(@"
                        INSERT INTO db_igaming.bets (user_id, amount, details)
                        VALUES (@UserId, @Amount, @Details);

                    ", bet, transaction);
        }
    }
}

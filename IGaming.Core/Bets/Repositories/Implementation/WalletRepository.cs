using Dapper;
using IGaming.Core.Bets.Dtos;
using IGaming.Core.Bets.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace IGaming.Core.Bets.Repositories.Implementation
{
    public class WalletRepository : IWalletRepository
    {
        public  async Task UpdateAsync(IDbConnection con, WalletDto walletDto, IDbTransaction? dbTransaction = null)
        {
            await con.ExecuteAsync(@"                        
                        UPDATE db_igaming.wallet
                        SET balance = balance - @Amount,
                            update_date_utc = now()
                        WHERE user_id = @UserId;", walletDto, dbTransaction);
        }

        public async Task<decimal> GetBalanceAsync(IDbConnection con, int userId, IDbTransaction? transaction = null)
        {
            var balance = await con.QuerySingleOrDefaultAsync<decimal>(@"
                    SET SESSION time_zone = '+00:00';
                    SELECT balance FROM db_igaming.wallet
                    WHERE user_id = @UserId;
                    ", new { UserId = userId }, transaction);
            return balance;
        }
    }
}

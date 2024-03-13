using IGaming.Core.Bets.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.Bets.Repositories.Interfaces
{
    public interface IWalletRepository
    {
        Task<decimal> GetBalanceAsync(IDbConnection con, int userId, IDbTransaction? transaction = null);
        Task UpdateAsync(IDbConnection con, WalletDto walletDto, IDbTransaction? dbTransaction = null);
    }
}

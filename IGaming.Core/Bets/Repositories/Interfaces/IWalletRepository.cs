using IGaming.Core.Bets.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.Bets.Repositories.Interfaces
{
    /// <summary>
    /// Interface for wallet repository operations.
    /// </summary>
    public interface IWalletRepository
    {
        /// <summary>
        /// Retrieves the balance of a user's wallet asynchronously.
        /// </summary>
        /// <param name="con">The database connection.</param>
        /// <param name="userId">The ID of the user whose balance is to be retrieved.</param>
        /// <param name="transaction">The optional database transaction.</param>
        /// <returns>A task representing the asynchronous operation. The balance of the user's wallet.</returns>
        Task<decimal> GetBalanceAsync(IDbConnection con, int userId, IDbTransaction? transaction = null);
        /// <summary>
        /// Updates the balance of a user's wallet asynchronously.
        /// </summary>
        /// <param name="con">The database connection.</param>
        /// <param name="walletDto">The wallet DTO containing the user ID and new balance.</param>
        /// <param name="dbTransaction">The optional database transaction.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(IDbConnection con, WalletDto walletDto, IDbTransaction? dbTransaction = null);
    }
}

using IGaming.Core.Bets.Dtos;
using IGaming.Core.Bets.RequestModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.Bets.Repositories.Interfaces
{
    /// <summary>
    /// Interface for bets repository operations.
    /// </summary>
    public interface IBetsRepository
    {
        /// <summary>
        /// Inserts a bet asynchronously into the database.
        /// </summary>
        /// <param name="con">The database connection.</param>
        /// <param name="bet">The bet DTO containing information about the bet.</param>
        /// <param name="transaction">The optional database transaction.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task InsertAsync(IDbConnection con, BetDto bet, IDbTransaction? transaction = null);

    }
}

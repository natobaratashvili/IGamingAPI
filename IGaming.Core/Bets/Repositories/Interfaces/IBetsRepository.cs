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
    public interface IBetsRepository
    {
        Task InsertAsync(IDbConnection con, BetDto bet, IDbTransaction? transaction = null);

    }
}

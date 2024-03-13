using IGaming.Core.Bets.Dtos;
using IGaming.Core.Bets.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.Bets.Mappers
{
    public static class BetRequestToDtoMapper
    {
        public static BetDto ToBetDto(this PlaceBetRequest placeBet, int userId)
        {
            return new BetDto()
            {
                UserId = userId,
                Amount = placeBet.Amount,
                Details = placeBet.Details,

            };
        }
    }
}

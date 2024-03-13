using IGaming.Core.Bets.RequestModels;
using IGaming.Core.Common;

namespace IGaming.Core.Bets.Services.Interfaces
{
    public interface IBetsManagementService
    {
        Task<Result> PlaceAsync(PlaceBetRequest placeBet, string userName, CancellationToken cancellationToken);
    }
}

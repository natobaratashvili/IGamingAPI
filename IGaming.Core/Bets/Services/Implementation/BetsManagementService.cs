using IGaming.Core.Bets.Dtos;
using IGaming.Core.Bets.Mappers;
using IGaming.Core.Bets.Repositories.Interfaces;
using IGaming.Core.Bets.RequestModels;
using IGaming.Core.Bets.Services.Interfaces;
using IGaming.Core.Common;
using IGaming.Core.Database;
using IGaming.Core.UsersManagement.ResponseModels;
using IGaming.Core.UsersManagement.Services.Interfaces;

namespace IGaming.Core.Bets.Services.Implementation
{
    /// <summary>
    /// Service for managing bets.
    /// </summary>
    public class BetsManagementService : IBetsManagementService
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="BetsManagementService"/> class.
        /// </summary>
        /// <param name="betsRepository">The bets repository.</param>
        /// <param name="userManagementService">The user management service.</param>
        /// <param name="dbWrapper">The database wrapper.</param>
        /// <param name="walletRepository">The wallet repository.</param>
        private readonly IBetsRepository _betsRepository;
        private readonly IUserManagementService _userManagementService;
        private readonly IDbWrapper _db;
        private readonly IWalletRepository _walletRepository;
        public BetsManagementService(IBetsRepository betsRepository, IUserManagementService userManagementService, IDbWrapper dbWrapper, IWalletRepository walletRepository)
        {
            _betsRepository = betsRepository;
            _db = dbWrapper;
            _userManagementService = userManagementService;
            _walletRepository = walletRepository;
        }
        /// <summary>
        /// Places a bet asynchronously.
        /// </summary>
        /// <param name="placeBet">The place bet request.</param>
        /// <param name="username">The username of the user placing the bet.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation. The result of the bet placement attempt.</returns>
        /// <remarks>
        /// This method first retrieves the user's profile to get the user ID.
        /// It then validates if the user has sufficient balance in their wallet to place the bet.
        /// If the balance is sufficient, it inserts the bet into the database and updates the wallet balance.
        /// </remarks>
        public async Task<Result> PlaceAsync(PlaceBetRequest placeBet, string username, CancellationToken cancellationToken)
        {
            var resultUser = await _userManagementService.GetProfileAsync(username, cancellationToken);
            if (resultUser is Result<UserProfileResponse> resultData)
            {
                var bet = placeBet.ToBetDto(resultData.Data.Id);
             var result =  await _db.RunWithTransactionAsync(async (dbconnection, transaction) =>
                {
                    var balance = await _walletRepository.GetBalanceAsync(dbconnection, bet.UserId, transaction);
                    if (balance < placeBet.Amount) return Result.Failure("Amount", "Not enough balance", 400);
                    await _betsRepository.InsertAsync(dbconnection, bet, transaction);
                    await _walletRepository.UpdateAsync(dbconnection, new WalletDto(bet.UserId, bet.Amount));
                    return Result.Success(204);
                }, cancellationToken);
                return result;

            }
            return resultUser;
            
        }
    }
}

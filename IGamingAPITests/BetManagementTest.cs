using IGaming.Core.Bets.Dtos;
using IGaming.Core.Bets.Repositories.Interfaces;
using IGaming.Core.Bets.RequestModels;
using IGaming.Core.Bets.Services.Implementation;
using IGaming.Core.Bets.Services.Interfaces;
using IGaming.Core.Common;
using IGaming.Core.Database;
using IGaming.Core.UsersManagement.ResponseModels;
using IGaming.Core.UsersManagement.Services.Interfaces;
using Moq;
using NUnit.Framework;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

[TestFixture]
public class BetManagementTest
{
    private IBetsManagementService _betService;
    private Mock<IUserManagementService> _userManagementServiceMock;
    private Mock<IDbWrapper> _dbMock;
    private Mock<IWalletRepository> _walletRepositoryMock;
    private Mock<IBetsRepository> _betsRepositoryMock;

    [SetUp]
    public void SetUp()
    {
        _userManagementServiceMock = new Mock<IUserManagementService>();
        _dbMock = new Mock<IDbWrapper>();
        _walletRepositoryMock = new Mock<IWalletRepository>();
        _betsRepositoryMock = new Mock<IBetsRepository>();
        _betService = new BetsManagementService(_betsRepositoryMock.Object, _userManagementServiceMock.Object, _dbMock.Object, _walletRepositoryMock.Object);
    }

    [Test]
    public async Task PlaceAsync_NotEnoughBalance_ReturnsFailResult()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var username = "testUser";
        var placeBetRequest =  new PlaceBetRequest(Amount: 50, "test");
        var userProfileResponse = new UserProfileResponse(Id: 1, null, null, null, 0, 0);
        var betDto = new BetDto();

        _userManagementServiceMock
            .Setup(x => x.GetProfileAsync(username, cancellationToken))
            .ReturnsAsync(Result<UserProfileResponse>.Success(userProfileResponse));

        var id =  await _userManagementServiceMock.Object.GetProfileAsync(username, cancellationToken);
        _walletRepositoryMock
            .Setup(x => x.GetBalanceAsync(It.IsAny<DbConnection>(), userProfileResponse.Id, It.IsAny<DbTransaction>()))
            .ReturnsAsync(20);

        _betsRepositoryMock
            .Setup(x => x.InsertAsync(It.IsAny<DbConnection>(), It.IsAny<BetDto>(), It.IsAny<DbTransaction>()))
            .Returns(Task.CompletedTask);

        _walletRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<DbConnection>(), It.IsAny<WalletDto>(), It.IsAny<DbTransaction>()))
            .Returns(Task.CompletedTask);
 
        _dbMock
            .Setup(x => x.RunWithTransactionAsync(It.IsAny<Func<IDbConnection, IDbTransaction, Task<Result>>>(), cancellationToken))
            .Returns((Func<DbConnection, DbTransaction, Task<Result>> func, CancellationToken token) => func(Mock.Of<DbConnection>(), Mock.Of<DbTransaction>()))
            .Verifiable();


        // Act
        var result = await _betService.PlaceAsync(placeBetRequest, username, cancellationToken);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.GetStatusCode(), Is.EqualTo(400));
    }
    [Test]
    public async Task PlaceAsync_WithEnoughBalance_ReturnsSuccessResult()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var username = "testUser";
        var placeBetRequest = new PlaceBetRequest(Amount: 50, "test");
        var userProfileResponse = new UserProfileResponse(Id: 1, null, null, null, 0, 0);
        var betDto = new BetDto();

        _userManagementServiceMock
            .Setup(x => x.GetProfileAsync(username, cancellationToken))
            .ReturnsAsync(Result<UserProfileResponse>.Success(userProfileResponse));

        var id = await _userManagementServiceMock.Object.GetProfileAsync(username, cancellationToken);
        _walletRepositoryMock
            .Setup(x => x.GetBalanceAsync(It.IsAny<DbConnection>(), userProfileResponse.Id, It.IsAny<DbTransaction>()))
            .ReturnsAsync(100);

        _betsRepositoryMock
            .Setup(x => x.InsertAsync(It.IsAny<DbConnection>(), It.IsAny<BetDto>(), It.IsAny<DbTransaction>()))
            .Returns(Task.CompletedTask);

        _walletRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<DbConnection>(), It.IsAny<WalletDto>(), It.IsAny<DbTransaction>()))
            .Returns(Task.CompletedTask);

        _dbMock
            .Setup(x => x.RunWithTransactionAsync(It.IsAny<Func<IDbConnection, IDbTransaction, Task<Result>>>(), cancellationToken))
            .Returns((Func<DbConnection, DbTransaction, Task<Result>> func, CancellationToken token) => func(Mock.Of<DbConnection>(), Mock.Of<DbTransaction>()))
            .Verifiable();


        // Act
        var result = await _betService.PlaceAsync(placeBetRequest, username, cancellationToken);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.GetStatusCode(), Is.EqualTo(204));
    }
}

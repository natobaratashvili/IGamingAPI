using Dapper;
using IGaming.Core.DatabaseAccessHelpers;
using IGaming.Core.UsersManagement.Dtos;
using IGaming.Core.UsersManagement.Repositories.Interfaces;


namespace IGaming.Core.UsersManagement.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;
        public UserRepository(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }
        public async Task CreateAsync(UserProfileDto user, CancellationToken cancellationToken)
        {
            using var connection = await _dbConnectionProvider.CreateConnectionAsync(cancellationToken);

            using var transaction =  connection.BeginTransaction();
            try
            {
                var userId = await connection.ExecuteScalarAsync<int>(@"
                        SET SESSION time_zone = '+00:00';
                        INSERT INTO db_igaming.users
                        ( email,username,guid,password)
                        VALUES(@Email, @Username, @Guid, @HashedPassword);
                        SELECT LAST_INSERT_ID() as id;
                    ", user, transaction);

                await connection.ExecuteAsync(@"
                     INSERT INTO db_igaming.wallet
                        (user_id)
                        VALUES(@UserId);

                    ",new {UserId = userId}, transaction);
                transaction.Commit();

            } catch (Exception)
            {
                transaction.Rollback();
                
                throw;
            }
        }

        public async Task<UserProfileDto> GetAsync(string username, CancellationToken cancellationToken)
        {
            using var connection = await _dbConnectionProvider.CreateConnectionAsync(cancellationToken);
            var profile = await connection.QuerySingleOrDefaultAsync<UserProfileDto>(@"
                SELECT u.id, u.guid, u.email, u.username, w.balance, u.password as HashedPassword, u.create_date_utc as CreateDateAtUtc, COALESCE(SUM(b.amount), 0) as TotalBet
                FROM db_igaming.users AS u
                LEFT JOIN db_igaming.wallet AS w ON w.user_id = u.id
                LEFT JOIN db_igaming.bets AS b ON b.user_id = u.id
                WHERE u.username = @Username
                GROUP BY u.id, u.email, u.username, w.balance;             
                ", new {Username = username});
            return profile;
        }
    }
}

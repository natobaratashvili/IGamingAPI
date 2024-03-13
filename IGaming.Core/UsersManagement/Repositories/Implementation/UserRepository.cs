using Dapper;
using IGaming.Core.DatabaseAccessHelpers;
using IGaming.Core.UsersManagement.Dtos;
using IGaming.Core.UsersManagement.Repositories.Interfaces;
using System.Data;
using System.Threading;

namespace IGaming.Core.UsersManagement.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        public async Task CreateAsync(IDbConnection con, UserProfileDto user, IDbTransaction? transaction = null)
        {
                await con.ExecuteAsync(@"
                        SET SESSION time_zone = '+00:00';
                        INSERT INTO db_igaming.users
                        ( email,username,guid,password)
                        VALUES(@Email, @Username, @Guid, @HashedPassword);
                        SELECT LAST_INSERT_ID() as id;
                    ", user, transaction);
                  var userId =  await con.ExecuteScalarAsync<int>(@"
                    select id 
                    FROM db_igaming.users
                    where email = @Email;
                ", user, transaction);
                await con.ExecuteAsync(@"
                     INSERT INTO db_igaming.wallet
                        (user_id)
                        VALUES(@UserId);

                    ", new { UserId = userId }, transaction);
                      
          
        }
      

        public async Task<UserProfileDto> GetByEmailAsync(IDbConnection con, string email, IDbTransaction? transaction = null)
        {
            var profile = await con.QuerySingleOrDefaultAsync<UserProfileDto>(@"
                SELECT u.id, u.guid, u.email, u.username, w.balance, u.password as HashedPassword, u.create_date_utc as CreateDateAtUtc, COALESCE(SUM(b.amount), 0) as TotalBet
                FROM db_igaming.users AS u
                LEFT JOIN db_igaming.wallet AS w ON w.user_id = u.id
                LEFT JOIN db_igaming.bets AS b ON b.user_id = u.id
                WHERE u.email = @Email
                GROUP BY u.id, u.email, u.username, w.balance;             
                ", new { Email = email }, transaction);
            return profile;
        }

        public async Task<UserProfileDto> GetByUserNameAsync(IDbConnection con, string username, IDbTransaction? transaction = null)
        {
            var profile = await con.QuerySingleOrDefaultAsync<UserProfileDto>(@"
                SELECT u.id, u.guid, u.email, u.username, w.balance, u.password as HashedPassword, u.create_date_utc as CreateDateAtUtc, COALESCE(SUM(b.amount), 0) as TotalBet
                FROM db_igaming.users AS u
                LEFT JOIN db_igaming.wallet AS w ON w.user_id = u.id
                LEFT JOIN db_igaming.bets AS b ON b.user_id = u.id
                WHERE u.username = @Username
                GROUP BY u.id, u.email, u.username, w.balance;             
                ", new { Username = username }, transaction);
            return profile;
        }
    }
}

using IGaming.Core.UsersManagement.Dtos;
using IGaming.Core.UsersManagement.RequestModels;
using System.Data;

namespace IGaming.Core.UsersManagement.Repositories.Interfaces
{
    /// <summary>
    /// Interface for user repository operations.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Creates a new user profile in the database.
        /// </summary>
        /// <param name="con">The database connection.</param>
        /// <param name="user">The user profile data to create.</param>
        /// <param name="transaction">The optional database transaction.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateAsync(IDbConnection con, UserProfileDto user, IDbTransaction? transaction = null);
        /// <summary>
        /// Retrieves a user profile by email from the database.
        /// </summary>
        /// <param name="con">The database connection.</param>
        /// <param name="email">The email address of the user.</param>
        /// <param name="transaction">The optional database transaction.</param>
        /// <returns>A task representing the asynchronous operation. The user profile if found, otherwise null.</returns>
        Task<UserProfileDto> GetByEmailAsync(IDbConnection con, string email, IDbTransaction? transaction = null);
        /// <summary>
        /// Retrieves a user profile by username from the database.
        /// </summary>
        /// <param name="con">The database connection.</param>
        /// <param name="username">The username of the user.</param>
        /// <param name="transaction">The optional database transaction.</param>
        /// <returns>A task representing the asynchronous operation. The user profile if found, otherwise null.</returns>
        Task<UserProfileDto> GetByUserNameAsync(IDbConnection con, string username, IDbTransaction? transaction = null);

    }
}

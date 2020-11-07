using Contador.Core.Models;

namespace Contador.DAL.Repositories
{
    /// <summary>
    /// Manages users in db.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets user by provided id.
        /// </summary>
        /// <param name="id">Id of requested user.</param>
        /// <returns>User of provided id.</returns>
        User GetUserById(int id);
    }
}
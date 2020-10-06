﻿using Contador.Core.Models;
using Contador.DAL.Repositories.Interfaces;

namespace Contador.DAL.Repositories
{
    /// <summary>
    /// Manages users in db.
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        /// <summary>
        /// Gets user by provided id.
        /// </summary>
        /// <param name="id">Id of requested user.</param>
        /// <returns>User of provided id.</returns>
        public User GetUserById(int id)
        {
            return new User() { Name = "Marysia" };
        }
    }
}

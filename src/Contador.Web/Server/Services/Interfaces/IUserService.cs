﻿using Contador.Core.Models;

namespace Contador.Web.Server.Services
{
    /// <summary>
    /// Users manager.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets user by provided id.
        /// </summary>
        /// <param name="id">Id of requested user.</param>
        /// <returns>User of provided id.</returns>
        User GetUserById(int id);
    }
}

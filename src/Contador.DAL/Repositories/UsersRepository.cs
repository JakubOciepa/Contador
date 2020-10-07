using Contador.Core.Models;
using Contador.DAL.Repositories.Interfaces;
using System.Collections.Generic;

namespace Contador.DAL.Repositories
{
    /// <summary>
    /// Manages users in db.
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        private readonly List<User> _stub;

        /// <summary>
        /// Creates new instance of <see cref="UsersRepository"/> class.
        /// </summary>
        public UsersRepository()
        {
            _stub = new List<User>
            {
                new User { Id = 0, Name = "Marysia", Email = "m@o.com"},
            };
        }

        ///<inheritdoc/>
        public User GetUserById(int id)
        {
            return _stub.Find(x => x.Id == id);
        }
    }
}

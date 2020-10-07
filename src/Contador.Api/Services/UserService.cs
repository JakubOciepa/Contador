using Contador.Core.Models;
using Contador.DAL.Repositories;

namespace Contador.Api.Services
{
    /// <inheritdoc/>
    public class UserService : IUserService
    {
        private readonly IUsersRepository _repository;

        /// <summary>
        /// Creates instance of <see cref="UserService"/> class.
        /// </summary>
        /// <param name="repository">Users repository.</param>
        public UserService(IUsersRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public User GetUserById(int id)
        {
            return _repository.GetUserById(id);
        }
    }
}

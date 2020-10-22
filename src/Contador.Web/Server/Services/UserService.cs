using Contador.Core.Models;
using Contador.DAL.Repositories;

namespace Contador.Web.Server.Services
{
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
            var user = _repository.GetUserById(id);

            return user != null
                ? new User() { Name = user.Name, Email = user.Email }
                : default;
        }
    }
}

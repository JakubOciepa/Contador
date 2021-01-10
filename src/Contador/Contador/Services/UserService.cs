using Contador.Abstractions;
using Contador.Core.Models;
using Contador.DAL.Abstractions;

namespace Contador.Services
{
	/// <inheritdoc/>
	public class UserService : IUserService
	{
		private readonly IUserRepository _repository;

		/// <summary>
		/// Creates instance of <see cref="UserService"/> class.
		/// </summary>
		/// <param name="repository">Users repository.</param>
		public UserService(IUserRepository repository)
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

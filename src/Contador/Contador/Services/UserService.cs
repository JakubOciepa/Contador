using Contador.Abstractions;
using Contador.Core.Models;
using Contador.DAL.Abstractions;

namespace Contador.Services
{
	/// <summary>
	/// Users manager.
	/// </summary>
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

		/// <summary>
		/// Gets user by provided id.
		/// </summary>
		/// <param name="id">Id of requested user.</param>
		/// <returns>User of provided id.</returns>
		public User GetUserById(int id)
		{
			var user = _repository.GetUserById(id);

			return user != null
				? new User() { Name = user.Name, Email = user.Email }
				: default;
		}
	}
}

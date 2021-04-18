using System.Collections.Generic;

using Contador.Core.Models;
using Contador.DAL.Abstractions;

namespace Contador.DAL.MySql.Repositories
{
	/// <summary>
	/// Manages users in db.
	/// </summary>
	public class UserRepository : IUserRepository
	{
		private readonly List<User> _stub;

		/// <summary>
		/// Creates new instance of <see cref="UserRepository"/> class.
		/// </summary>
		public UserRepository()
		{
			_stub = new List<User>
			{
				new User { Id = "0", UserName = "Marysia", Email = "m@o.com"},
			};
		}

		/// <summary>
		/// Gets user by provided id.
		/// </summary>
		/// <param name="id">Id of requested user.</param>
		/// <returns>User of provided id.</returns>
		public User GetUserById(string id)
		{
			return _stub.Find(x => x.Id == id);
		}
	}
}

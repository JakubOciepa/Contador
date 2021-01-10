using System.Collections.Generic;

using Contador.Core.Models;
using Contador.DAL.Abstractions;

namespace Contador.DAL.Repositories
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

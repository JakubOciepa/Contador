using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Contador.DAL.SQLite.Models;
using Contador.Mobile.Common;

using SQLite;

namespace Contador.Mobile.DAL
{
	/// <summary>
	/// Database connection class.
	/// </summary>
	public class DbConnection
	{
		////data/user/0/com.companyname.contador.mobile/files/.local/share/Test.db3
		private static readonly Lazy<SQLiteAsyncConnection> _lazyInitializer = new Lazy<SQLiteAsyncConnection>(
			() => new SQLiteAsyncConnection(Config.Db.Path, Config.Db.Flags));

		private static bool _initialized = false;

		/// <summary>
		/// Add here db types so tables will be created on start!
		/// </summary>
		private readonly List<Type> types = new List<Type>()
		{
			typeof(ExpenseDto),
			typeof(ExpenseCategoryDto)
		};

		/// <summary>
		/// <see cref="SQLiteAsyncConnection"/> connection.
		/// </summary>
		public static SQLiteAsyncConnection Database => _lazyInitializer.Value;

		/// <summary>
		/// Creates instance of the <see cref="DbConnection"/> class.
		/// </summary>
		public DbConnection()
		{
			InitializeAsync().ConfigureAwait(false);
		}

		private async Task InitializeAsync()
		{
			if (!_initialized)
			{
				try
				{
					foreach (var type in types)
					{
						if (!Database.TableMappings.Any(m => m.MappedType.Name == type.Name))
						{
							await Database.CreateTablesAsync(CreateFlags.None, type).ConfigureAwait(false);
						}
					}

					_initialized = true;
				}
				catch
				{
					_initialized = false;
				}
			}
		}
	}
}

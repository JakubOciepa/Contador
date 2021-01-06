using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Contador.DAL.SQLite.Models;
using Contador.Mobile.Common;

using SQLite;

namespace Contador.Mobile.DAL
{
    public class DbConnection
    {
        private static readonly Lazy<SQLiteAsyncConnection> _lazyInitializer = new Lazy<SQLiteAsyncConnection>(
            () => new SQLiteAsyncConnection(Config.Db.Path, Config.Db.Flags));

        private static bool _initialized = false;

        private readonly List<Type> types = new List<Type>()
        {
            typeof(ExpenseDto),
        };

        public static SQLiteAsyncConnection Database => _lazyInitializer.Value;

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

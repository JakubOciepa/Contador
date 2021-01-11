using System;

namespace Contador.Mobile.Common
{
	/// <summary>
	/// Most common configurations.
	/// </summary>
	public static class Config
	{
		/// <summary>
		/// Database configuration.
		/// </summary>
		public static class Db
		{
			/// <summary>
			/// Database file name.
			/// </summary>
			public static string Name => "Test.db3";

			/// <summary>
			/// <see cref="SQLite.SQLiteOpenFlags"/> flags.
			/// </summary>
			public const SQLite.SQLiteOpenFlags Flags =
				// open the database in read/write mode
				SQLite.SQLiteOpenFlags.ReadWrite |
				// create the database if it doesn't exist
				SQLite.SQLiteOpenFlags.Create |
				// enable multi-threaded database access
				SQLite.SQLiteOpenFlags.SharedCache;

			/// <summary>
			/// Path to the database file.
			/// </summary>
			public static string Path
			{
				get
				{
					var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
					return System.IO.Path.Combine(basePath, Name);
				}
			}
		}
	}
}

using System;

namespace Contador.Mobile.Common
{
	public static class Config
	{
		public static class Db
		{
			public static string Name => "Test.db3";

			public const SQLite.SQLiteOpenFlags Flags =
				// open the database in read/write mode
				SQLite.SQLiteOpenFlags.ReadWrite |
				// create the database if it doesn't exist
				SQLite.SQLiteOpenFlags.Create |
				// enable multi-threaded database access
				SQLite.SQLiteOpenFlags.SharedCache;

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

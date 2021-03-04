using Contador.Abstractions;
using Contador.Core.Common;

using Serilog;

//[assembly: Dependency(typeof(Contador.Mobile.Droid.Services.Log))]
namespace Contador.Mobile.Droid.Services
{
	/// <summary>
	/// Provides basic logging functionality.
	/// </summary>
	public class Log : ILog
	{
		/// <summary>
		/// Creates an instance of the <see cref="Log"/> class.
		/// </summary>
		public Log()
		{
			Serilog.Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.Enrich.FromLogContext()
				.WriteTo.AndroidLog()
				.CreateLogger();
		}

		/// <summary>
		/// Writes message of specific type into target output.
		/// </summary>
		/// <param name="level">Log level of the message.</param>
		/// <param name="message">Message to write out.</param>
		public void Write(LogLevel level, string message)
		{
			switch (level)
			{
				case LogLevel.Info:
					Serilog.Log.Logger.Information(message);
					break;

				case LogLevel.Warning:
					Serilog.Log.Logger.Warning(message);
					break;

				case LogLevel.Error:
					Serilog.Log.Logger.Error(message);
					break;

				case LogLevel.Fatal:
					Serilog.Log.Fatal(message);
					break;
			}
		}
	}
}

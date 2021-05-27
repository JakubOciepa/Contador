
using Contador.Services.Interfaces;

namespace Contador.Web.Client.Services
{
	/// <summary>
	/// Provides basic logging functionality.
	/// </summary>
	public class Log : ILog
	{

		/// <summary>
		/// Creates instance of the <see cref="Log"/> class.
		/// </summary>
		/// <param name="logger">Logging provider;</param>
		public Log()
		{

		}

		/// <summary>
		/// Writes message of specific type into target output.
		/// </summary>
		/// <param name="level">Log level of the message.</param>
		/// <param name="message">Message to write out.</param>
		public void Write(Core.Common.LogLevel level, string message)
		{
			switch (level)
			{
				case Core.Common.LogLevel.Info:
					Serilog.Log.Information(message);
					break;

				case Core.Common.LogLevel.Warning:
					Serilog.Log.Warning(message);
					break;

				case Core.Common.LogLevel.Error:
					Serilog.Log.Error(message);
					break;

				case Core.Common.LogLevel.Fatal:
					Serilog.Log.Fatal(message);
					break;
			}
		}
	}
}

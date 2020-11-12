using Contador.Abstractions;
using Contador.Core.Common;

using Serilog;

using Xamarin.Forms;

[assembly: Dependency(typeof(Contador.Mobile.Droid.Services.Log))]
namespace Contador.Mobile.Droid.Services
{
    /// <inheritdoc/>
    public class Log : ILog
    {
        /// <inheritdoc/>
        public Log()
        {
            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.AndroidLog()
                .CreateLogger();
        }

        ///<inheritdoc/>
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


using Serilog;

namespace Contador.Api.Services
{
    public static class Log
    {
        public static readonly ILogger Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(@"logs\currentLog.log", rollingInterval: RollingInterval.Month)
            .CreateLogger();
    }
}

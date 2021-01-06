using Contador.Abstractions;

using Microsoft.Extensions.Logging;

namespace Contador.Web.Server.Services
{
    /// <inheritdoc/>
    public class Log : ILog
    {
        private readonly ILogger<Program> _logger;

        /// <summary>
        /// Creates instance of the <see cref="Log"/> class.
        /// </summary>
        /// <param name="logger">Logging provider;</param>
        public Log(ILogger<Program> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public void Write(Core.Common.LogLevel level, string message)
        {
            switch (level)
            {
                case Core.Common.LogLevel.Info:
                    _logger.LogInformation(message);
                    break;

                case Core.Common.LogLevel.Warning:
                    _logger.LogWarning(message);
                    break;

                case Core.Common.LogLevel.Error:
                    _logger.LogError(message);
                    break;

                case Core.Common.LogLevel.Fatal:
                    _logger.LogCritical(message);
                    break;
            }
        }
    }
}


using Contador.Core.Common;

namespace Contador.Abstractions
{
    /// <summary>
    /// Provides basic logging functionality.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Writes message of specific type into target output.
        /// </summary>
        /// <param name="level">Log level of the message.</param>
        /// <param name="message">Message to write out.</param>
        public void Write(LogLevel level, string message);
    }
}

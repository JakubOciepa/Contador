using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Contador.Core.Utils.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ConfiguredTaskAwaitable{TResult}"/> class.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Adds ConfigureAwait(false) to <see cref="Task<T>"./>
        /// </summary>
        /// <typeparam name="T">Type of task return.</typeparam>
        /// <param name="task">Target task of this extension.</param>
        /// <returns><see cref="ConfiguredTaskAwaitable"/> task.</returns>
        public static ConfiguredTaskAwaitable<T> CAF<T>(this Task<T> task)
        {
            return task.ConfigureAwait(false);
        }
    }
}

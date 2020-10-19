namespace Contador.Core.Common
{
    /// <summary>
    /// Result that contains returned object and proper response code which indicates if execution were ok.
    /// </summary>
    /// <typeparam name="TResult">Returned object type.</typeparam>
    public class Result<TResult>
        where TResult : class
    {
        /// <summary>
        /// Execution result code.
        /// </summary>
        public System.Enum ResponseCode { get; }

        /// <summary>
        /// Returned object.
        /// </summary>
        public TResult ReturnedObject { get; }

        /// <summary>
        /// Additional message which describes execution result.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Creates new instance of <see cref="Result" /> object.
        /// </summary>
        /// <param name="code">Execution result code.</param>
        /// <param name="obj">Returned object.</param>
        public Result(System.Enum code, TResult obj)
        {
            ResponseCode = code;
            ReturnedObject = obj;
        }
    }
}

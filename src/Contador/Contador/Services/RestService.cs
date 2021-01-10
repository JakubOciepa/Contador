using System;
using System.Net.Http;
using System.Threading.Tasks;

using Contador.Abstractions;
using Contador.Core.Common;
using Contador.Core.Models;
using Contador.Core.Utils.Extensions;

using Newtonsoft.Json;

namespace Contador.Services
{
	/// <inheritdoc/>
	public class RestService : IRestService
	{
		private const string REST_ADDR = "http://192.168.1.31:5000/api/";
		private const string EXPENSE_ADDR = "expense";

		private readonly ILog _logger;

		public RestService(ILog logger)
		{
			_logger = logger;
		}

		/// <inheritdoc/>
		public async Task<Result<Expense>> GetExpenseByIdAsync(int id)
		{
			using var client = new HttpClient();

			var result = await client.GetAsync($"{REST_ADDR}{EXPENSE_ADDR}/{id}").CAF();
			if (result.StatusCode != System.Net.HttpStatusCode.OK)
			{
				//TODO: get correct status code from HttpStatusCode.
				return new Result<Expense>(ResponseCode.Error, default);
			}

			try
			{
				var jsonContent = await result.Content.ReadAsStringAsync().CAF();

				return new Result<Expense>(ResponseCode.Ok, JsonConvert.DeserializeObject<Expense>(jsonContent));
			}
			catch (Exception ex)
			{
				_logger?.Write(Core.Common.LogLevel.Warning, $"Can not convert JSON into Expense.\n {ex.StackTrace}.");
				return default;
			}
		}
	}
}

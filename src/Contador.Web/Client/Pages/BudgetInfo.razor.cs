using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Contador.Core.Models;
using Contador.Services.Interfaces;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Contador.Web.Client.Pages
{
	public partial class BudgetInfo
	{
		[Parameter] public string Id { get; set; }

		[Inject] private HttpClient _httpClient { get; set; }
		[Inject] private ILog _logger { get; set; }
		[Inject] private IJSRuntime _jsRuntime { get; set; }


		public Budget Budget { get; set; } = new Budget();

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

			Budget = await GetBudgetAsync(Convert.ToInt32(Id));
		}

		private async Task<Budget> GetBudgetAsync(int id)
		{
			try
			{
				var result = await _httpClient.GetAsync($"api/budget/{id}");

				if (result.IsSuccessStatusCode && result.StatusCode is not HttpStatusCode.NoContent)
				{
					return (await result.Content.ReadFromJsonAsync<Budget>());
				}

				return new Budget();
			}
			catch (Exception ex)
			{
				_logger.Write(Core.Common.LogLevel.Error, $"{ex.Message}:\n{ex.StackTrace}");

				return new Budget();
			}
		}
	}
}

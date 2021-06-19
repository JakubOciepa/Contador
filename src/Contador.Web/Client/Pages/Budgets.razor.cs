﻿using System;
using System.Collections.Generic;
using System.Linq;
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
	public partial class Budgets
	{
		[Inject] private HttpClient _httpClient { get; set; }
		[Inject] private ILog _logger { get; set; }
		[Inject] private IJSRuntime _jsRuntime { get; set; }

		public List<Budget> BudgetList { get; set; } = new List<Budget>();

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

			BudgetList = await GetBudgets();
		}

		private async Task<List<Budget>> GetBudgets()
		{
			try
			{
				var result = await _httpClient.GetAsync("api/budget");

				if (result.IsSuccessStatusCode && result.StatusCode is not HttpStatusCode.NoContent)
				{
					return (await result.Content.ReadFromJsonAsync<IList<Budget>>())
						.OrderBy(e => e.StartDate).ToList();
				}

				return new List<Budget>();
			}
			catch (Exception ex)
			{
				_logger.Write(Core.Common.LogLevel.Error, $"{ex.Message}:\n{ex.StackTrace}");

				return new List<Budget>();
			}
		}
	}
}

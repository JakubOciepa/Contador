using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Contador.Core.Models;
using Contador.Services.Interfaces;
using Contador.Web.Client.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Contador.Web.Client.Pages
{
	public partial class Categories
	{
		[Inject] private HttpClient _httpClient { get; set; }
		[Inject] private ILog _logger { get; set; }
		[Inject] private IJSRuntime _jsRuntime { get; set; }


		private IList<ExpenseCategory> CategoriesList = new List<ExpenseCategory>();
		private ExpenseCategoryModel CategoryModel = new();

		protected override async Task OnInitializedAsync()
		{
			CategoriesList = await GetAndSortCategories();
		}

		private async void AddNewCategory()
		{
			var request = new HttpRequestMessage(HttpMethod.Post, "api/expensecategory");

			try
			{
				request.Content = GetHttpStringContent();

				var result = await _httpClient.SendAsync(request);

				if (result is HttpResponseMessage response && response.IsSuccessStatusCode)
				{
					CategoriesList = await GetAndSortCategories();

					this.StateHasChanged();

					CategoryModel = new();
				}
				else if (result.StatusCode is HttpStatusCode.Conflict
					|| result.StatusCode is HttpStatusCode.BadRequest)
				{
					_logger.Write(Core.Common.LogLevel.Error, "Cannot add category!");
					await _jsRuntime.InvokeVoidAsync("alert", "Cannot add category!");
				}
			}
			catch (Exception ex)
			{
				_logger.Write(Core.Common.LogLevel.Error, $"{ex.Message}:\n{ex.StackTrace}");
				await _jsRuntime.InvokeVoidAsync("alert", "Cannot add category!");
			}
		}

		private void RemoveCategoryFromCategoryList(ExpenseCategory categoryToRemove)
		{
			CategoriesList.Remove(categoryToRemove);
		}

		private async Task<IList<ExpenseCategory>> GetAndSortCategories()
		{
			try
			{
				var result = await _httpClient.GetAsync("api/expensecategory");

				if (result.IsSuccessStatusCode && result.StatusCode is not HttpStatusCode.NoContent)
				{
					return (await result.Content.ReadFromJsonAsync<IList<ExpenseCategory>>())
						.OrderByDescending(e => e.Name).ToList();
				}

				return new List<ExpenseCategory>();
			}
			catch (Exception ex)
			{
				_logger.Write(Core.Common.LogLevel.Error, $"{ex.Message}:\n{ex.StackTrace}");

				return new List<ExpenseCategory>();
			}
		}

		private StringContent GetHttpStringContent()
		{
			var body = new
			{
				name = CategoryModel.Name,
			};

			return new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
		}
	}
}

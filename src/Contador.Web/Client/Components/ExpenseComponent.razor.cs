
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Contador.Abstractions;
using Contador.Core.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Contador.Web.Client.Components
{
	public partial class ExpenseComponent
	{
		[Inject] private HttpClient _httpClient { get; set; }
		[Inject] private ILog _logger { get; set; }
		[Inject] private IJSRuntime _jsRuntime { get; set; }

		private bool isEditMode = false;

		/// <summary>
		/// Expense which will be displayed and managed by the component.
		/// </summary>
		[Parameter]
		public Expense Expense { get; set; }

		/// <summary>
		/// Action that will be invoked after correct removing the expense.
		/// </summary>
		[Parameter]
		public EventCallback<Expense> OnExpenseRemoved { get; set; }

		private string Name { get; set; }
		private decimal Value { get; set; }
		private int CategoryId { get; set; }
		private string Description { get; set; }
		private DateTime CreatedDate { get; set; }
		private List<ExpenseCategory> Categories { get; set; }

		protected override async Task OnInitializedAsync()
		{
			if (Expense is not null)
			{
				Name = Expense.Name;
				Value = Expense.Value;
				CategoryId = Expense.Category.Id;
				Description = Expense.Description;
				CreatedDate = Expense.CreateDate;
			}

			Categories = await GetCategories();
		}

		private void EnterEditMode()
			=> isEditMode = true;

		private async void RemoveExpense()
		{
			var removeConfirmed = await _jsRuntime.InvokeAsync<bool>("confirm", "Do you really want to remove this expense?");

			if (removeConfirmed)
			{
				try
				{
					var request = new HttpRequestMessage(HttpMethod.Delete, $"api/expense/{Expense.Id}");
					var result = await _httpClient.SendAsync(request);

					if (result.IsSuccessStatusCode)
					{
						await OnExpenseRemoved.InvokeAsync(Expense);
					}
				}
				catch (Exception ex)
				{
					_logger.Write(Core.Common.LogLevel.Warning, $"Can't remove {Expense.Name}\n: {ex.StackTrace}");
				}
			}
		}

		private void ExitEditMode()
			=> isEditMode = false;

		private async Task UpdateExpenseAsync()
		{
			var request = new HttpRequestMessage(HttpMethod.Put, $"api/expense/{Expense.Id}");

			try
			{
				request.Content = GetHttpStringContent();

				var result = await _httpClient.SendAsync(request);

				if (result is HttpResponseMessage response && response.IsSuccessStatusCode)
				{
					Expense = await GetExpense(Expense.Id);

					await _jsRuntime.InvokeVoidAsync("alert", "Expense has been updated!");

					this.StateHasChanged();

				}
				else if (result.StatusCode is HttpStatusCode.Conflict
					|| result.StatusCode is HttpStatusCode.BadRequest)
				{
					_logger.Write(Core.Common.LogLevel.Error, "Cannot update expense!");
					await _jsRuntime.InvokeVoidAsync("alert", "Cannot update expense!");
				}
			}
			catch (Exception ex)
			{
				_logger.Write(Core.Common.LogLevel.Error, $"{ex.Message}:\n{ex.StackTrace}");
				await _jsRuntime.InvokeVoidAsync("alert", "Cannot update expense!");
			}
			finally
			{
				isEditMode = false;
			}

		}

		private StringContent GetHttpStringContent()
		{
			var body = new
			{
				id = Expense.Id,
				name = Name,
				category = new
				{
					id = CategoryId,
					name = Categories.First(c => c.Id == CategoryId).Name,
				},
				user = new
				{
					id = 1,
					name = "Kuba",
					email = "kuba@test.com"
				},
				value = Value,
				description = Description,
				imagePath = "",
				createDate = CreatedDate.ToString("yyyy-MM-ddTHH:mm:ss")
			};

			return new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
		}

		private async Task<Expense> GetExpense(int id)
		{
			try
			{
				var result = await _httpClient.GetAsync($"api/expense/{id}");

				if (result.IsSuccessStatusCode && result.StatusCode is not HttpStatusCode.NoContent)
				{
					return (await result.Content.ReadFromJsonAsync<Expense>());
				}

				return null;
			}
			catch (Exception ex)
			{
				_logger.Write(Core.Common.LogLevel.Error, $"{ex.Message}:\n{ex.StackTrace}");

				return null;
			}
		}

		private async Task<List<ExpenseCategory>> GetCategories()
		{
			var result = await _httpClient.GetAsync("api/expensecategory");

			if (result.IsSuccessStatusCode && result.StatusCode is not HttpStatusCode.NoContent)
			{
				return await result.Content.ReadFromJsonAsync<List<ExpenseCategory>>();
			}

			return new List<ExpenseCategory>();
		}
	}
}

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
using Contador.Web.Client.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using Microsoft.JSInterop;

namespace Contador.Web.Client.Pages
{
	public partial class Expenses
	{
		[Inject] private HttpClient _httpClient { get; set; }
		[Inject] private ILog _logger { get; set; }
		[Inject] private IJSRuntime _jsRuntime { get; set; }
		[Inject] private AuthenticationStateProvider _authenticationStateProvider { get; set; }

		private IList<Expense> ExpensesList = new List<Expense>();
		private IList<ExpenseCategory> Categories = new List<ExpenseCategory>();
		private ExpenseModel ExpenseModel = new();
		private string Filter = "";

		private bool _sortByNameDescending = false;
		private bool _sortByCategoryDescending = false;
		private bool _sortByUserDescending = false;
		private bool _sortByValueDescending = false;
		private bool _sortByDateDescending = false;

		protected override async Task OnInitializedAsync()
		{
			ExpensesList = await GetAndSortExpenses();
			Categories = await GetCategories();
		}

		private async Task AddNewExpense()
		{
			var request = new HttpRequestMessage(HttpMethod.Post, "api/expense");

			try
			{
				request.Content = await GetHttpStringContent();

				var result = await _httpClient.SendAsync(request);

				if (result is HttpResponseMessage response && response.IsSuccessStatusCode)
				{
					ExpensesList = await GetAndSortExpenses();

					this.StateHasChanged();

				}
				else if (result.StatusCode is HttpStatusCode.Conflict
					|| result.StatusCode is HttpStatusCode.BadRequest)
				{
					_logger.Write(Core.Common.LogLevel.Error, "Cannot add expense!");
					await _jsRuntime.InvokeVoidAsync("alert", "Cannot add expense!");
				}
			}
			catch (Exception ex)
			{
				_logger.Write(Core.Common.LogLevel.Error, $"{ex.Message}:\n{ex.StackTrace}");
				await _jsRuntime.InvokeVoidAsync("alert", "Cannot add expense!");
			}
		}

		private bool IsVisible(Expense expense)
		{
			if (string.IsNullOrEmpty(Filter))
				return true;
			if (expense.Name.Contains(Filter, StringComparison.OrdinalIgnoreCase))
				return true;
			if (expense.Category.Name.Contains(Filter, StringComparison.OrdinalIgnoreCase))
				return true;
			if (expense.User.UserName.Contains(Filter, StringComparison.OrdinalIgnoreCase))
				return true;
			if (expense.Description.Contains(Filter, StringComparison.OrdinalIgnoreCase))
				return true;

			return false;
		}

		private void SortBy(string propertyName)
		{
			if (propertyName is "Name")
			{
				SortByName();
			}
			else if (propertyName is "Category")
			{
				SortByCategory();
			}
			else if (propertyName is "Value")
			{
				SortByValue();
			}
			else if (propertyName is "CreateDate")
			{
				SortByDate();
			}
			else if (propertyName is "User")
			{
				SortByUser();
			}
		}

		private void SortByUser()
		{
			if (_sortByUserDescending)
			{
				ExpensesList = ExpensesList.OrderBy(e => e.User.UserName).ToList();
				_sortByUserDescending = false;
			}
			else
			{
				ExpensesList = ExpensesList.OrderByDescending(e => e.User.UserName).ToList();
				_sortByUserDescending = true;
			}
		}

		private void SortByDate()
		{
			if (_sortByDateDescending)
			{
				ExpensesList = ExpensesList.OrderByDescending(e => e.CreateDate).ToList();
				_sortByDateDescending = false;
			}
			else
			{
				ExpensesList = ExpensesList.OrderBy(e => e.CreateDate).ToList();
				_sortByDateDescending = true;
			}
		}

		private void SortByValue()
		{
			if (_sortByValueDescending)
			{
				ExpensesList = ExpensesList.OrderByDescending(e => e.Value).ToList();
				_sortByValueDescending = false;
			}
			else
			{
				ExpensesList = ExpensesList.OrderBy(e => e.Value).ToList();
				_sortByValueDescending = true;
			}
		}

		private void SortByCategory()
		{
			if (_sortByCategoryDescending)
			{
				ExpensesList = ExpensesList.OrderByDescending(e => e.Category.Name).ToList();
				_sortByCategoryDescending = false;
			}
			else
			{
				ExpensesList = ExpensesList.OrderBy(e => e.Category.Name).ToList();
				_sortByCategoryDescending = true;
			}
		}

		private void SortByName()
		{
			if (_sortByNameDescending)
			{
				ExpensesList = ExpensesList.OrderByDescending(e => e.Name).ToList();
				_sortByNameDescending = false;
			}
			else
			{
				ExpensesList = ExpensesList.OrderBy(e => e.Name).ToList();
				_sortByNameDescending = true;
			}
		}

		private void RemoveExpenseFromExpenseList(Expense expenseToRemove)
		{
			ExpensesList.Remove(expenseToRemove);
		}

		private async Task<StringContent> GetHttpStringContent()
		{
			var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
			var result = await _httpClient.GetAsync($"api/account/{authState.User.Identity.Name}");
			var user = await result.Content.ReadFromJsonAsync<User>();

			var body = new
			{
				name = ExpenseModel.Name,
				category = new
				{
					id = ExpenseModel.CategoryId,
					name = Categories.First(c => c.Id == ExpenseModel.CategoryId).Name,
				},
				user = new
				{
					id = user.Id,
					name = user.UserName,
					email = user.Email
				},
				value = ExpenseModel.Value,
				description = ExpenseModel.Description,
				imagePath = "",
				createDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
			};

			return new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
		}

		private async Task<IList<Expense>> GetAndSortExpenses()
		{
			try
			{
				var result = await _httpClient.GetAsync("api/expense");

				if (result.IsSuccessStatusCode && result.StatusCode is not HttpStatusCode.NoContent)
				{
					return (await result.Content.ReadFromJsonAsync<IList<Expense>>())
						.OrderByDescending(e => e.CreateDate).ToList();
				}

				return new List<Expense>();
			}
			catch (Exception ex)
			{
				_logger.Write(Core.Common.LogLevel.Error, $"{ex.Message}:\n{ex.StackTrace}");

				return new List<Expense>();
			}
		}

		private async Task<IList<ExpenseCategory>> GetCategories()
		{
			var result = await _httpClient.GetAsync("api/expensecategory");

			if (result.IsSuccessStatusCode && result.StatusCode is not HttpStatusCode.NoContent)
			{
				return await result.Content.ReadFromJsonAsync<IList<ExpenseCategory>>();
			}

			return new List<ExpenseCategory>();
		}
	}
}

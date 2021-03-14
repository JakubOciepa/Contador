
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Contador.Core.Models;

using Microsoft.AspNetCore.Components;

namespace Contador.Web.Client.Components
{
	public partial class ExpenseComponent
	{
		[Inject] private HttpClient _httpClient { get; set; }

		private bool isEditMode = false;

		[Parameter]
		public Expense Expense { get; set; }

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

		private void EnterEditMode() => isEditMode = true;

		private void ExitEditMode() => isEditMode = false;

		private void UpdateExpense() { }

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

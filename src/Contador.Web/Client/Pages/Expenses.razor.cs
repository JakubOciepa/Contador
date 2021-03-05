using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Contador.Core.Models;

using Microsoft.AspNetCore.Components;

namespace Contador.Web.Client.Pages
{
	public partial class Expenses
	{
		[Inject]
		private HttpClient _httpClient { get; set; }

		private IList<Expense> expenses;
		private IList<ExpenseCategory> categories;

		protected override async Task OnInitializedAsync()
		{
			try
			{
				expenses = await _httpClient.GetFromJsonAsync<IList<Expense>>("api/expense");
				categories = await _httpClient.GetFromJsonAsync<IList<ExpenseCategory>>("api/expensecategory");
			}
			catch(Exception ex)
			{

			}

		}

	}
}

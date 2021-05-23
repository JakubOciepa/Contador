using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Contador.Core.Models;
using Contador.Services.Interfaces;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Contador.Web.Client.Components
{
	public partial class ExpenseCategoryComponent
	{
		[Inject] private HttpClient _httpClient { get; set; }
		[Inject] private ILog _logger { get; set; }
		[Inject] private IJSRuntime _jsRuntime { get; set; }

		private bool isEditMode = false;

		/// <summary>
		/// Category which will be displayed and managed by the component.
		/// </summary>
		[Parameter]
		public ExpenseCategory Category { get; set; }

		/// <summary>
		/// Action that will be invoked after correct removing the category.
		/// </summary>
		[Parameter]
		public EventCallback<ExpenseCategory> OnCategoryRemoved { get; set; }

		private string Name { get; set; }

		protected override void OnInitialized()
		{
			if (Category is not null)
			{
				Name = Category.Name;
			}
		}

		private void EnterEditMode()
		{
			isEditMode = true;
		}

		private async void RemoveCategory()
		{
			var removeConfirmed = await _jsRuntime.InvokeAsync<bool>("confirm", "Do you really want to remove this category?");

			if (removeConfirmed)
			{
				try
				{
					var request = new HttpRequestMessage(HttpMethod.Delete, $"api/expensecategory/{Category.Id}");
					var result = await _httpClient.SendAsync(request);

					if (result.IsSuccessStatusCode)
					{
						await OnCategoryRemoved.InvokeAsync(Category);
					}
				}
				catch (Exception ex)
				{
					_logger.Write(Core.Common.LogLevel.Warning, $"Can't remove {Category.Name}\n: {ex.StackTrace}");
				}
			}
		}

		private void ExitEditMode()
		{
			isEditMode = false;
		}

		private async Task UpdateCategoryAsync()
		{
			var request = new HttpRequestMessage(HttpMethod.Put, $"api/expensecategory/{Category.Id}");

			try
			{
				request.Content = GetHttpStringContent();

				var result = await _httpClient.SendAsync(request);

				if (result is HttpResponseMessage response && response.IsSuccessStatusCode)
				{
					Category = await GetCategory(Category.Id);

					await _jsRuntime.InvokeVoidAsync("alert", "Category has been updated!");

					this.StateHasChanged();

				}
				else if (result.StatusCode is HttpStatusCode.Conflict
					|| result.StatusCode is HttpStatusCode.BadRequest)
				{
					_logger.Write(Core.Common.LogLevel.Error, "Cannot update category!");
					await _jsRuntime.InvokeVoidAsync("alert", "Cannot update category!");
				}
			}
			catch (Exception ex)
			{
				_logger.Write(Core.Common.LogLevel.Error, $"{ex.Message}:\n{ex.StackTrace}");
				await _jsRuntime.InvokeVoidAsync("alert", "Cannot update category!");
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
				id = Category.Id,
				name = Name,
			};

			return new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
		}

		private async Task<ExpenseCategory> GetCategory(int id)
		{
			try
			{
				var result = await _httpClient.GetAsync($"api/expensecategory/{id}");

				if (result.IsSuccessStatusCode && result.StatusCode is not HttpStatusCode.NoContent)
				{
					return await result.Content.ReadFromJsonAsync<ExpenseCategory>();
				}

				return null;
			}
			catch (Exception ex)
			{
				_logger.Write(Core.Common.LogLevel.Error, $"{ex.Message}:\n{ex.StackTrace}");

				return null;
			}
		}
	}
}

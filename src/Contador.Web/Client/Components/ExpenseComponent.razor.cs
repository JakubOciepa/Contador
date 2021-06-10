
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Contador.Core.Models;
using Contador.Services.Interfaces;
using Contador.Web.Client.Pages;
using Contador.Web.Client.Services;
using Contador.Web.Shared.Files;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Contador.Web.Client.Components
{
	/// <summary>
	/// Component that shows and manage expense.
	/// </summary>
	public partial class ExpenseComponent
	{
		[Inject] private HttpClient _httpClient { get; set; }
		[Inject] private ILog _logger { get; set; }
		[Inject] private IJSRuntime _jsRuntime { get; set; }
		[Inject] private AuthenticationStateProvider _authenticationStateProvider { get; set; }
		[Inject] private FilesManager _fileManager { get; set; }

		private bool isEditMode = false;
		private bool uploading = false;

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

		/// <summary>
		/// Expense page parameter.
		/// </summary>
		[CascadingParameter]
		public Expenses ExpensesPage { get; set; }

		private string Name { get; set; }
		private decimal Value { get; set; }
		private int CategoryId { get; set; }
		private string Description { get; set; }
		private DateTime CreatedDate { get; set; }
		private List<ExpenseCategory> Categories { get; set; }
		private bool IsSelected { get; set; }

		protected override async Task OnInitializedAsync()
		{
			if (Expense is not null)
			{
				Name = Expense.Name;
				Value = Expense.Value;
				CategoryId = Expense.Category?.Id ?? -1;
				Description = Expense.Description;
				CreatedDate = Expense.CreateDate;
			}

			Categories = await GetCategories();
			ExpensesPage.DeselectAllExpenses += ExpensesPage_DeselectAllExpenses;
		}

		private void ExpensesPage_DeselectAllExpenses(object sender, EventArgs e)
		{
			IsSelected = false;
			InvokeAsync(StateHasChanged);
		}

		private void EnterEditMode()
			=> isEditMode = true;

		private void OnSelectionChanged(object args)
		{
			if ((bool)args)
			{
				ExpensesPage.ExpenseSelectionChanged((bool)args, Expense);
			}
		}

		private async Task OnInputFileChange(InputFileChangeEventArgs args)
		{
			try
			{
				uploading = true;
				await InvokeAsync(StateHasChanged);

				var format = "image/png";
				var imageFile = await args.File.RequestImageFileAsync(format, 480, 640);


				var buffer = new byte[imageFile.Size];
				await imageFile.OpenReadStream().ReadAsync(buffer);

				string fileName = Path.GetFileNameWithoutExtension(args.File.Name);
				string newFileName = $"{fileName.ToString()}-{DateTime.Now.Ticks.ToString()}.png";

				var chunk = new FileChunk
				{
					Data = buffer,
					FileNameNoPath = newFileName,
					Offset = 0,
					FirstChunk = true
				};

				var result = await _fileManager.UploadFileChunk(chunk);

				if(result is true)
				{
					Expense.ImagePath = newFileName;
				}

				uploading = false;

			}
			catch (Exception ex)
			{
				uploading = false;
				_logger.Write(Core.Common.LogLevel.Error, ex.ToString());
			}
		}

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
				request.Content = await GetHttpStringContent();

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

		private async Task<StringContent> GetHttpStringContent()
		{
			var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
			var result = await _httpClient.GetAsync($"api/account/{authState.User.Identity.Name}");
			var user = await result.Content.ReadFromJsonAsync<User>();

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
					id = user.Id,
					name = user.UserName,
					email = user.Email
				},
				value = Value,
				description = Description,
				imagePath = Expense.ImagePath,
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

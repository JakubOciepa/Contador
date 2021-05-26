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

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Contador.Web.Client.Pages
{
	public partial class Issues
	{
		[Inject] private HttpClient _httpClient { get; set; }
		[Inject] private ILog _logger { get; set; }
		[Inject] private IJSRuntime _jsRuntime { get; set; }

		public List<Issue> IssueList { get; set; } = new List<Issue>();

		public Issue IssueToAdd { get; set; } = new ();

		protected override async Task OnInitializedAsync()
		{
			IssueList = await GetIssues();
		}

		public async Task AddNewIssue()
		{
			var request = new HttpRequestMessage(HttpMethod.Post, "api/issue");

			try
			{
				var body = new
				{
					name = IssueToAdd.Name,
				};

				request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

				var result = await _httpClient.SendAsync(request);

				if (result is HttpResponseMessage response && response.IsSuccessStatusCode)
				{
					IssueList = await GetIssues();

					this.StateHasChanged();

				}
				else if (result.StatusCode is HttpStatusCode.Conflict
					|| result.StatusCode is HttpStatusCode.BadRequest)
				{
					_logger.Write(Core.Common.LogLevel.Error, "Cannot add issue!");
					await _jsRuntime.InvokeVoidAsync("alert", "Cannot add issue!");
				}
			}
			catch (Exception ex)
			{
				_logger.Write(Core.Common.LogLevel.Error, $"{ex.Message}:\n{ex.StackTrace}");
				await _jsRuntime.InvokeVoidAsync("alert", "Cannot add issue!");
			}
		}

		private async Task<List<Issue>> GetIssues()
		{
			var result = await _httpClient.GetAsync("api/issue");

			if (result.IsSuccessStatusCode && result.StatusCode is not HttpStatusCode.NoContent)
			{
				return (await result.Content.ReadFromJsonAsync<IList<Issue>>())
					.OrderByDescending(e => e.CreateDate).ToList();
			}

			return new List<Issue>();
		}
	}
}

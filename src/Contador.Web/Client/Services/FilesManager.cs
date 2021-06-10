using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Contador.Web.Shared.Files;

namespace Contador.Web.Client.Services
{
	public class FilesManager
	{
		private readonly HttpClient _client;

		public FilesManager(HttpClient client)
		{
			_client = client;
		}

		public async Task<bool> UploadFileChunk(FileChunk fileChunk)
		{
			try
			{
				var result = await _client.PostAsJsonAsync("api/files", fileChunk);
				result.EnsureSuccessStatusCode();
				string responseBody = await result.Content.ReadAsStringAsync();

				return Convert.ToBoolean(responseBody);
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}
}

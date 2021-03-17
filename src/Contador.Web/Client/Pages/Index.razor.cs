using System.Net.Http;

using Microsoft.AspNetCore.Components;

namespace Contador.Web.Client.Pages
{
	public partial class Index
	{
		[Inject] private HttpClient _httpClient { get; set; }
	}
}


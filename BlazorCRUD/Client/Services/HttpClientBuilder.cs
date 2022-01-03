using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;

namespace BlazorCRUD.Client.Services
{
	public interface IHttpClientBuilder
	{
		Task<HttpClient> Build();
	}

	public class HttpClientBuilder : IHttpClientBuilder
	{
		ITokenProvider _tokenProvider;
		string baseAddress;
		public HttpClientBuilder(NavigationManager navManager, ITokenProvider tokenProvider)
		{
			baseAddress = navManager.BaseUri;
			_tokenProvider = tokenProvider;
		}

		public async Task<HttpClient> Build()
		{
			HttpClient client = new();
			client.BaseAddress = new Uri(baseAddress);
			var token = _tokenProvider.GetToken();

			client.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue(
					"Bearer", await token
				);
			return client;
		}
	}
}

using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;

namespace BlazorCRUD.Client.Services
{
	public delegate void OnLogin(string User);
	public delegate void OnLogout();
	public interface ITokenProvider
	{
		Task<string?> GetToken();
		Task<string?> GetUser();
		Task Set(string? _token);
		OnLogin OnLogin { get; set; }
		OnLogout OnLogout { get; set; }
	}

	public class TokenProvider : ITokenProvider
	{
		public OnLogin OnLogin { get; set; } = delegate { };
		public OnLogout OnLogout { get; set; } = delegate { };
		private Task Init { get; set; }
		private async Task Initialize()
		{
			token = await storage.Get("Token");
		}

		public TokenProvider(ILocalStorage _storage, NavigationManager _nav)
		{
			storage = _storage;
			_baseUri = _nav.BaseUri;
			Init = Initialize();
		}

		private readonly ILocalStorage storage;
		private readonly string _baseUri;
		private string? token;

		public async Task<string?> GetToken()
		{
			await Init;
			return token;
		}

		public async Task<string?> GetUser()
		{
			await Init;
			if (token != null) {
				try {
					using var client = new HttpClient() { BaseAddress = new(_baseUri) };
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
					return await client.GetStringAsync("User/GetUser");
				} catch (HttpRequestException) {
					token = null;
					storage.Clear();
					return null;
				}
			} else {
				return null;
			}
		}

		public async Task Set(string? _token)
		{
			await Init;
			token = _token;
			// storage.Set("User", user);
			if (_token != null) {
				storage.Set("Token", _token);
				OnLogin((await GetUser())!);
			} else {
				storage.Remove("Token");
				OnLogout();
			}
		}
	}
}


using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorCRUD.Client.Services
{
	public delegate void OnSet();
	public class UserAuthenticationService
	{
		public OnSet OnSet { get; set; } = delegate { };
		private Task Init { get; set; }
		private async Task Initialize()
		{
			await Set(await storage.Get("Token"));
		}

		public UserAuthenticationService(ILocalStorage _storage, NavigationManager _nav)
		{
			storage = _storage;
			baseUri = _nav.BaseUri;
			Init = Initialize();
		}

		private readonly ILocalStorage storage;
		private readonly string baseUri;
		private string? token;
		private string? username;

		public async Task<string?> GetToken()
		{
			await Init;
			return token;
		}

		public async Task<string?> GetUsername()
		{
			await Init;
			return username;
		}

		public async Task Set(string? _token)
		{
			token = _token;
			if (!string.IsNullOrWhiteSpace(token)) {
				using var client = BuildAuthenticatedHttpClient();
				var response = await client.GetAsync("user/getuser");
				if (response.IsSuccessStatusCode) {
					username = await response.Content.ReadAsStringAsync();
					storage.Set("Token", token);
				} else {
					token = null;
				}
			}
			if (string.IsNullOrWhiteSpace(token)) {
				storage.Remove("Token");
			}
			OnSet();
		}

		public async Task<HttpClient> BuildAuthenticatedHttpClientAsync()
		{
			await Init;
			return BuildAuthenticatedHttpClient();
		}

		private HttpClient BuildAuthenticatedHttpClient()
		{
			HttpClient client = new();
			client.BaseAddress = new Uri(baseUri);
			client.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("Bearer", token);
			return client;
		}

		public AuthenticationStateProvider BuildAuthenticationStateProvider() =>
			new AppAuthenticationStateProvider(this);

		private class AppAuthenticationStateProvider : AuthenticationStateProvider
		{
			private readonly UserAuthenticationService provider;

			public AppAuthenticationStateProvider(UserAuthenticationService _provider)
			{
				provider = _provider;
				provider.OnSet += StateChanged;
			}

			public override async Task<AuthenticationState> GetAuthenticationStateAsync()
			{
				var savedToken = await provider.GetToken();

				if (string.IsNullOrWhiteSpace(savedToken)) {
					return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
				}

				return new AuthenticationState(new ClaimsPrincipal(
						new ClaimsIdentity(ParseClaimsFromJwt(savedToken), "jwt"))
				);
			}

			public async void StateChanged()
			{
				var username = await provider.GetUsername();
				var identity = string.IsNullOrEmpty(username) ?
					new ClaimsIdentity() :
					new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, "apiauth");
				NotifyAuthenticationStateChanged(
					Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)))
				);
			}

			private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
			{
				var claims = new List<Claim>();
				var payload = jwt.Split('.')[1];
				var pairs = JsonSerializer.Deserialize<Dictionary<string, object>>(
					Convert.FromBase64String((payload.Length % 4) switch
					{
						2 => payload + "==",
						3 => payload + "=",
						_ => payload,
					})
				) ?? throw new ArgumentException("Unable to deserialize jwt");

				if (pairs.TryGetValue(ClaimTypes.Role, out object? roles)) {
					if (roles.ToString()!.Trim().StartsWith("[")) {
						var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString()!);
						foreach (var parsedRole in parsedRoles!) {
							claims.Add(new Claim(ClaimTypes.Role, parsedRole));
						}
					} else {
						claims.Add(new Claim(ClaimTypes.Role, roles.ToString()!));
					}
					pairs.Remove(ClaimTypes.Role);
				}

				claims.AddRange(pairs.Select(pair => new Claim(pair.Key, pair.Value.ToString()!)));
				return claims;
			}
		}
	}
}
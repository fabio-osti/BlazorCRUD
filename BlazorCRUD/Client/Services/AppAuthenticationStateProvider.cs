
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorCRUD.Client.Services
{
	public class AppAuthenticationStateProvider : AuthenticationStateProvider
	{
		private readonly ITokenProvider _provider;

		public AppAuthenticationStateProvider(ITokenProvider provider)
		{
			_provider = provider;
			_provider.OnLogin += MarkUserAsLoggedIn;
			_provider.OnLogout += MarkUserAsLoggedOut;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var savedToken = await _provider.GetToken();

			if (string.IsNullOrWhiteSpace(savedToken)) {
				return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
			}

			return new AuthenticationState(new ClaimsPrincipal(
					new ClaimsIdentity(ParseClaimsFromJwt(savedToken), "jwt")));
		}

		public void MarkUserAsLoggedIn(string user)
		{
			Console.WriteLine("Loggin in: " + user);
			var authenticatedUser =	new ClaimsPrincipal(new ClaimsIdentity(
				new[] { new Claim(ClaimTypes.Name, user) }, "apiauth"));
			var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
			NotifyAuthenticationStateChanged(authState);
		}

		public void MarkUserAsLoggedOut()
		{
			var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
			var authState = Task.FromResult(new AuthenticationState(anonymousUser));
			NotifyAuthenticationStateChanged(authState);
		}

		private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
		{
			var claims = new List<Claim>();
			var payload = jwt.Split('.')[1];
			var jsonBytes = ParseBase64WithoutPadding(payload);
			var keyValuePairs =
				JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes)
				?? throw new ArgumentException("Unable to deserialize jwt");

			if (keyValuePairs.TryGetValue(ClaimTypes.Role, out object? roles)) {
				if (roles.ToString()!.Trim().StartsWith("[")) {
					var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString()!);

					foreach (var parsedRole in parsedRoles!) {
						claims.Add(new Claim(ClaimTypes.Role, parsedRole));
					}
				} else {
					claims.Add(new Claim(ClaimTypes.Role, roles.ToString()!));
				}

				keyValuePairs.Remove(ClaimTypes.Role);
			}

			claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!)));

			return claims;
		}

		private static byte[] ParseBase64WithoutPadding(string base64)
		{
			switch (base64.Length % 4) {
				case 2: base64 += "=="; break;
				case 3: base64 += "="; break;
			}
			return Convert.FromBase64String(base64);
		}
	}
}


using BlazorCRUD.Client.Services;
using BlazorCRUD.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace BlazorCRUD.Client.Pages
{
	public partial class Login : ComponentBase
	{
		protected override void OnInitialized()
		{
			//OnSuccess ??= new(() => _ = 0);
			OnSuccess ??= new(() => Console.WriteLine("OnSuccess not set"));

			if (Provider == null) throw new ArgumentNullException(nameof(Provider));
			if (HttpBuilder == null) throw new ArgumentNullException(nameof(HttpBuilder));
		}

		[Parameter] public Action? OnSuccess { private get; set; }
		[Inject] IJSRuntime? Js { get; set; }
		[Inject] ITokenProvider? Provider { get; set; }
		[Inject] IHttpClientBuilder? HttpBuilder { get; set; }

		private record LoginFormUser
		{
			[Required]
			[EmailAddress]
			public string? Email { get; set; }
			[Required]
			[DataType(DataType.Password)]
			public string? Password { get; set; }
		}

		private LoginFormUser UserModel { get; set; } = new();
		private async void OnLogin()
		{
			try {
				// Get user salt
				var http = await HttpBuilder!.Build();
				var saltResponse = await http.PostAsJsonAsync("User/GetSalt", UserModel.Email);
				var salt = await saltResponse.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
				try {
					// Hashes and Salts the password
					var hashed = PassHelper.SaltAndHash(UserModel.Password!, salt);
					var tokenResponse = await http.PostAsJsonAsync("User/LogIn", new {
						Email = UserModel.Email,
						HashedPassword = hashed
					});
					// Set the token
					await Provider!.Set(await tokenResponse.EnsureSuccessStatusCode().Content.ReadAsStringAsync());
					OnSuccess!();
					UserModel = new();
				} catch (HttpRequestException) {
					await Js!.InvokeVoidAsync("alert", "Incorrect password");
					UserModel.Password = "";
				}
			} catch (HttpRequestException) {
				await Js!.InvokeVoidAsync("alert", "Email not found");
				UserModel = new();
			}

			StateHasChanged();
		}
	}
}
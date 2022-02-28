using BlazorCRUD.Client.Services;
using BlazorCRUD.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace BlazorCRUD.Client.Pages
{
	public partial class Signup : ComponentBase
	{
		protected override void OnInitialized()
		{
			//OnSuccess ??= new(() => _ = 0);
			OnSuccess ??= new(() => Console.WriteLine("OnSuccess not set"));

			if (User == null) throw new ArgumentNullException(nameof(User));
		}

		[Parameter] public Action? OnSuccess { private get; set; }
		[Inject] IJSRuntime? Js { get; set; }
		[Inject] private UserAuthenticationService? User { get; set; }

		private record SignupFormUser
		{
			[Required]
			[EmailAddress]
			public string? Email { get; set; }
			[Required]
			[MinLength(4)]
			public string? Username { get; set; }
			[Required]
			[DataType(DataType.Password)]
			public string? Password { get; set; }
		}

		private SignupFormUser UserModel { get; set; } = new();

		private async Task OnSignup()
		{
			try {
				var salt = PassHelper.GenSalt(24);
				var hashed = PassHelper.SaltAndHash(UserModel.Password!, salt);
				var Http = await User!.BuildAuthenticatedHttpClientAsync();
				using var response = await Http.PostAsJsonAsync("User/SignUp", new User
				{
					Username = UserModel.Username!,
					Email = UserModel.Email!,
					Password = hashed,
					Salt = salt,
				});
				await User!.Set(await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync());
				OnSuccess!();
			} catch (HttpRequestException) {
				await Js!.InvokeVoidAsync("alert", "Email already registered");
				UserModel = new();
			}
			StateHasChanged();
		}
	}
}
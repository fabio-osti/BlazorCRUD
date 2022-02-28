using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorCRUD.Client
{
	public partial class App
	{
		[CascadingParameter]
		private Task<AuthenticationState> AuthenticationStateTask { get; set; }

		private async Task LogUserAuthenticationState()
		{
			var authState = await AuthenticationStateTask;
			var user = authState.User;
			if (user.Identity!.IsAuthenticated) {
				Console.WriteLine($"User {user.Identity.Name} is authenticated.");
			} else {
				Console.WriteLine("User is NOT authenticated.");
			}
		}
	}
}
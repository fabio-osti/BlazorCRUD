using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using BlazorCRUD.Client;
using BlazorCRUD.Client.Shared;
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
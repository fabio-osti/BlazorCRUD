using BlazorCRUD.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorCRUD.Client.Shared
{
	public enum UserFormModal : byte
	{
		Closed, Login, Signup
	}
	public partial class LoginDisplay : ComponentBase
	{
		protected override async Task OnInitializedAsync()
		{
			Username = await TokenProvider!.GetUser();
		}

		[Inject] ITokenProvider? TokenProvider { get; set; }
		UserFormModal ModalState { get; set; }
		string? Username { get; set; }

		Action ModalActionBuilder(UserFormModal modal) => async () =>
		{
			ModalState = modal;
			Username = await TokenProvider!.GetUser();
			StateHasChanged();
		};
		Action OnClose => ModalActionBuilder(UserFormModal.Closed);
		
		private void LogOut(MouseEventArgs args)
		{
			TokenProvider!.Set(null);
		}
	}
}
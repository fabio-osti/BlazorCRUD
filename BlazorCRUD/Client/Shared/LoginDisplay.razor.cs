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
			Username = await User!.GetUsername();
		}

		[Inject] UserAuthenticationService? User { get; set; }
		UserFormModal ModalState { get; set; }
		string? Username { get; set; }

		Action ModalActionBuilder(UserFormModal modal) => async () =>
		{
			ModalState = modal;
			Username = await User!.GetUsername();
			StateHasChanged();
		};
		Action OnClose => ModalActionBuilder(UserFormModal.Closed);
		
		private void LogOut(MouseEventArgs args)
		{
			_ = User!.Set(null);
		}
	}
}
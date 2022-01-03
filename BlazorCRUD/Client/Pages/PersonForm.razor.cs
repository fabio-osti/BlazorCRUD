using BlazorCRUD.Client.Services;
using BlazorCRUD.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorCRUD.Client.Pages
{
	public partial class PersonForm : ComponentBase
	{
		[Parameter]
		public Person? PreFill { get; set; }
		[Parameter]
		public FormType? Type { get; set; } = FormType.Create;
		[Parameter]
		[EditorRequired]
		public Action<Person>? Success { get; set; }

		public void OnPersonSubmit()
		{
			Success!(PersonModel);
		}

		private string Label => Type switch
		{
			FormType.Create => "Add",
			FormType.Read => "Search",
			FormType.Update => "Edit",
			_ => ""
		};

		public Person PersonModel { get; set; } = new();
		protected override void OnParametersSet()
		{
			if (Success == null)
				throw new ArgumentNullException();

			if (PreFill != null)
				PersonModel = new(PreFill);
		}
	}
}

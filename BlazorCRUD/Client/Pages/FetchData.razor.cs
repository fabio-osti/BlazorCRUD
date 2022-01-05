using BlazorCRUD.Client.Services;

using BlazorCRUD.Shared;
using BlazorCRUD.Shared.Utilities;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using static BlazorCRUD.Shared.UtilitiesAlgorithims;

namespace BlazorCRUD.Client.Pages
{
	public enum FormType : byte
	{
		Closed, Create, Read, Update
	}

	public partial class FetchData : ComponentBase
	{
		[Inject] private IPersonStore Store { get; set; }
		private Person[]? Persons { get; set; }
		protected override async Task OnInitializedAsync()
		{
			await Get();
		}

		async Task Get()
		{
			var resp = await Store.Read(RowsPerPage, PageNum, OrderingBy, Filter);
			TotalEntries = resp.Count;
			Persons = resp.Response;
			StateHasChanged();
		}


		// Property to control the ordering
		private PersonOrdering orderingBy = PersonOrdering.None;
		private PersonOrdering OrderingBy
		{
			get => orderingBy;
			set
			{
				orderingBy = value;
				_ = Get();
			}
		}
		// Properties to control which caret should be shown and at which label
		private string NameCaret => orderingBy switch
		{
			PersonOrdering.NameA => "oi-caret-top",
			PersonOrdering.NameD => "oi-caret-bottom",
			_ => ""
		};
		private string AgeCaret => orderingBy switch
		{
			PersonOrdering.AgeA => "oi-caret-top",
			PersonOrdering.AgeD => "oi-caret-bottom",
			_ => ""
		};
		private string SexCaret => orderingBy switch
		{
			PersonOrdering.SexA => "oi-caret-top",
			PersonOrdering.SexD => "oi-caret-bottom",
			_ => ""
		};
		private string HairColorCaret => orderingBy switch
		{
			PersonOrdering.HairColorA => "oi-caret-top",
			PersonOrdering.HairColorD => "oi-caret-bottom",
			_ => ""
		};
		// Property to control the form modal
		private FormType form = FormType.Closed;
		private FormType Form
		{
			get => form;
			set
			{
				form = value;
				StateHasChanged();
			}
		}
		private Person? PersonProp => Form switch
		{
			FormType.Update => EditedPerson,
			FormType.Read => Filter,
			_ => null
		};
		// Property to be passed to the form when the edit button is clicked at an entry
		private Person? editedPerson;
		private Person? EditedPerson
		{
			get => editedPerson ?? new();
			set
			{
				editedPerson = value;
				Form = FormType.Update;
			}
		}
		// Property to be passed to the form when the search button is clicked
		private Person? filter;
		private Person? Filter
		{
			get => filter;
			set
			{
				filter = value != null && value.IsEmpty() ? null : value;
				_ = Get();
			}
		}
		// Properties to control the data table
		private int rowsPerPage = 10;
		private int RowsPerPage
		{
			get => rowsPerPage;
			set
			{
				rowsPerPage = value;
				_ = Get();
			}
		}

		private int pageNum = 1;
		private int PageNum
		{
			get => pageNum;
			set
			{
				pageNum = value.MinMax(0, TotalPages);
				_ = Get();
			}
		}

		private int TotalEntries { get; set; }
		private int TotalPages => (TotalEntries + RowsPerPage - 1) / RowsPerPage;
		// Actions
		private Action OnClose => () => Form = FormType.Closed;
		private Action<Person> OnSuccess =>
			Form switch
			{
				FormType.Read => (person) =>
				{
					Filter = person;
					Form = FormType.Closed;
				}
				,
				FormType.Create => async (person) =>
				{
					await Store.Create(person);
					await Get();
					Form = FormType.Closed;
				}
				,
				FormType.Update => async (person) =>
				{
					await Store.Update(person);
					await Get();
					Form = FormType.Closed;
				}
				,
				_ => throw new ArgumentException()
			};

		private Action OnAddClick => () => Form = FormType.Create;
		private Action OnSearchClick => () => Form = FormType.Read;
		private Action OnPageUp => () => PageNum++;
		private Action OnPageDown => () => PageNum--;
		private Action OnNameClick => () => OrderingBy = OrderingBy == PersonOrdering.NameA ? PersonOrdering.NameD : PersonOrdering.NameA;
		private Action OnAgeClick => () => OrderingBy = OrderingBy == PersonOrdering.AgeA ? PersonOrdering.AgeD : PersonOrdering.AgeA;
		private Action OnSexClick => () => OrderingBy = OrderingBy == PersonOrdering.SexA ? PersonOrdering.SexD : PersonOrdering.SexA;
		private Action OnHairColorClick => () => OrderingBy = OrderingBy == PersonOrdering.HairColorA ? PersonOrdering.HairColorD : PersonOrdering.HairColorA;
		private Action OnDeleteClickFactory(int id) => async () => await Store.Delete(id);
		private Action OnEditClickFactory(Person toEdit) => () => EditedPerson = toEdit;


	}
}

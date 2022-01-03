using BlazorCRUD.Shared;
using BlazorCRUD.Shared.Utilities;
using System.Net.Http.Json;

namespace BlazorCRUD.Client.Services
{
	public interface IPersonStore
	{
		Task<bool> Create(Person created);
		Task<bool> Delete(int id);
		Task<ApiResponse<Person>> Read(int take, int skip, PersonOrdering orderBy, Person? filter);
		Task<bool> Update(Person updated);
	}

	public class PersonStore : IPersonStore
	{
		private IHttpClientBuilder ClientBuilder { get; }

		public PersonStore(IHttpClientBuilder clientBuilder)
		{
			ClientBuilder = clientBuilder;
		}

		public async Task<bool> Create(Person created)
		{
			return
				(await (await ClientBuilder!.Build()).PostAsJsonAsync("person/create", created))
					.IsSuccessStatusCode;

		}

		public async Task<ApiResponse<Person>> Read(int take, int skip, PersonOrdering orderBy, Person? filter)
		{
			return await (await ClientBuilder!.Build()).GetFromJsonAsync<ApiResponse<Person>>(
				$"person/read?rows={take}&page={skip}&orderby={(byte)orderBy}" + (
					filter == null ?
						"" : (
							(filter.Name == null ? "" : $"&name={filter.Name}") +
							(filter.Age == null ? "" : $"&age={filter.Age}") +
							(filter.Sex == null ? "" : $"&sex={filter.Sex}") +
							(filter.HairColor == null ? "" : $"&hc={filter.HairColor}")
						)
				)
			) ?? new(Array.Empty<Person>(), 0);
		}

		public async Task<bool> Update(Person updated)
		{
			return
				(await (await ClientBuilder!.Build()).PostAsJsonAsync("person/update", updated))
					.IsSuccessStatusCode;
		}

		public async Task<bool> Delete(int id)
		{
			return
				(await (await ClientBuilder!.Build()).PostAsJsonAsync("person/delete", id))
					.IsSuccessStatusCode;
		}
	}
}

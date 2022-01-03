using Microsoft.JSInterop;

namespace BlazorCRUD.Client
{
	public interface ILocalStorage
	{
		Task<string> Get(string key);
		void Set(string key, string value);
		void Clear();
		void Remove(string key);
	}

	public class LocalStorage : ILocalStorage
	{
		private readonly Task<IJSObjectReference> jsModule;

		public LocalStorage(IJSRuntime _js)
		{
			jsModule =
				_js.InvokeAsync<IJSObjectReference>(
					"import",
					"./localStorage.js"
				).AsTask();
		}

		public async Task<string> Get(string key) =>
			await (await jsModule).InvokeAsync<string>("LocalStorageGet", key);
		public async void Set(string key, string value) =>
			await (await jsModule).InvokeVoidAsync("LocalStorageSet", key, value);
		public async void Remove(string key) =>
			await (await jsModule).InvokeVoidAsync("LocalStorageRemove", key);
		public async void Clear() =>
			await (await jsModule).InvokeVoidAsync("LocalStorageClear");
	}
}

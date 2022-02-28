using BlazorCRUD.Client;
using BlazorCRUD.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddAuthorizationCore();
builder.Services.AddSingleton<ILocalStorage, LocalStorage>();
builder.Services.AddSingleton<UserAuthenticationService>();
builder.Services.AddSingleton<IPersonStore, PersonStore>();
builder.Services.AddSingleton(
	e => e.GetRequiredService<UserAuthenticationService>().BuildAuthenticationStateProvider()
);


await builder.Build().RunAsync();

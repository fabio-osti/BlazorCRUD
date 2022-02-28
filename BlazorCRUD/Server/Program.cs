using BlazorCRUD.Server;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TokenAuthenticationHelper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
	.AddDbContext<PersonContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("AppDb")))
	.AddDbContext<UserContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("UsersDb")));
builder.Services.ConfigureTokenServices(Encoding.ASCII.GetBytes(builder.Configuration["TokenKey"]));
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.UseWebAssemblyDebugging();
} else {
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

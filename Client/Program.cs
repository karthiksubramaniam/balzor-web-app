using balzor_web_app.Client;
using balzor_web_app.Client.Authentication;
using balzor_web_app.Client.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<AppAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, AppAuthenticationStateProvider>();

await builder.Build().RunAsync();
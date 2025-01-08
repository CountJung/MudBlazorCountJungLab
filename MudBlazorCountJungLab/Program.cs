using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using MudBlazorCountJungLab;
using MudBlazorCountJungLab.Context;
using MudBlazorCountJungLab.Extensions;
using MudBlazorCountJungLab.Provider;
using MudBlazorCountJungLab.ViewModels;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddBlazorDownloadFile();
builder.Services.AddScoped<InstarPostViewModel>();
builder.Services.AddScoped<GoogleAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(p=>p.GetRequiredService<GoogleAuthenticationStateProvider>());
builder.Services.AddSingleton<JavaScriptModuleService>();
builder.Services.AddSingleton<GlobalContext>();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();

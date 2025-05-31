using BlazorFront.Auth;
using BlazorFront.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

namespace BlazorFront;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        var url = "https://localhost:7123";
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(url) });
        builder.Services.AddAuthorizationCore();
        builder.Services.AddMudServices();
        builder.Services.AddScoped<AuthenticationProviderJWT>();
        builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderJWT>();

        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<ILoginService, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
        builder.Services.AddScoped<IRepository, Repository>();
        builder.Services.AddBlazorBootstrap();

        await builder.Build().RunAsync();
    }
}

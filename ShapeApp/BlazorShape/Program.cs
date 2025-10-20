using BlazorShape.EnvironmentLoader;
using BlazorShape.Services;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using ShapeApi.Client;
using System;

namespace BlazorShape
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var root = Directory.GetCurrentDirectory();
            var dotenv = Path.Combine(root, ".env");
            DotEnv.Load(dotenv);

            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            var config = new ConfigurationBuilder().AddEnvironmentVariables("API_").Build();
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddSingleton<ShapeClient>();
            builder.Services.AddSingleton<ShapeService>();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            await builder.Build().RunAsync();
        }
    }
}

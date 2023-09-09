using Itmo.Dev.Asap.Frontend.Application.Abstractions;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Tools;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Extensions;
using Itmo.Dev.Asap.Frontend.Authentication.Extensions;
using Itmo.Dev.Asap.Frontend.Client;
using Itmo.Dev.Asap.Frontend.Integrations.Gateway.Extensions;
using Itmo.Dev.Asap.Frontend.LocalStorage.Extensions;
using Itmo.Dev.Asap.Frontend.Presentation.Markup.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Reflection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOptions();

builder.Services
    .AddFrontendApplication()
    .AddApplicationEvents(typeof(IAbstractionsAssemblyMarker))
    .ConfigureApplicationEvents(c => c.EnablePublishLogging = builder.HostEnvironment.IsDevelopment())
    .AddGatewayIntegration()
    .AddFrontendLocalStorage()
    .AddFrontendAuthentication()
    .AddMarkup(builder.HostEnvironment.IsDevelopment());

Type[] eagerDependencies = builder.Services
    .Where(x => x.Lifetime is ServiceLifetime.Singleton)
    .Where(x => x.ImplementationType?.IsAssignableTo(typeof(IEagerDependency)) is true)
    .OrderByDescending(x => x.ImplementationType?.GetCustomAttribute<EagerDependencyPriorityAttribute>()?.Priority ?? 0)
    .Select(x => x.ServiceType)
    .ToArray();

WebAssemblyHost host = builder.Build();

foreach (Type dependency in eagerDependencies)
{
    Type type = typeof(IEnumerable<>).MakeGenericType(dependency);
    _ = host.Services.GetRequiredService(type);
}

await host.Services.UseFrontendLocalStorageAsync();

await host.RunAsync();
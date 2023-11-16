using Microsoft.Extensions.Hosting;
using Serverless_Api.Middlewares;
using Infra.Migrations;
using Infra.Dependencies;
using Application.Dependencies;

var host = new HostBuilder()
    .ConfigureServices(services =>
    {
        services.AddEventStore();
        
        services.AddDomainDependencies();
        services.AddEventStoreDependencies();
        services.AddInfraDependencies();
        services.AddApplicationDependencies();
    })
    .ConfigureFunctionsWorkerDefaults(builder => builder.UseMiddleware<AuthMiddleware>())
    .Build();

host.Run();

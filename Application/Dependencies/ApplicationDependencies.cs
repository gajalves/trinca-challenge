using Application.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Dependencies
{
    public static class ApplicationDependencies
    {
        public static void AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateNewBbqHandler).Assembly));
        }
    }
}

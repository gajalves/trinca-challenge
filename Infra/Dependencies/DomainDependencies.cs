using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Dependencies
{
    public static class DomainDependencies
    {
        public static void AddDomainDependencies(this IServiceCollection services)
        {
            services.AddSingleton(new Person { Id = "e5c7c990-7d75-4445-b5a2-700df354a6a0" });
        }
    }
}

using Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Dependencies
{
    public static class InfraDependencies
    {
        public static void AddInfraDependencies(this IServiceCollection services)
        {
            services.AddTransient<IBbqRepository, BbqRepository>();
            services.AddTransient<IPersonRepository, PersonRepository>();
            services.AddTransient<IShoppingListRepository, ShoppingListRepository>();
        }
    }
}

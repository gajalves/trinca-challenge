using CrossCutting;
using Domain.Entities;
using Eveneum;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Dependencies
{
    public static class EventStoreDependencies
    {
        private const string DATABASE = "Churras";

        public static void AddEventStoreDependencies(this IServiceCollection services)
        {
            var client = new CosmosClient(Environment.GetEnvironmentVariable(nameof(EventStore)));

            var bbqStore = new EventStore<Bbq>(client, DATABASE, "Bbqs");
            bbqStore.Initialize().GetAwaiter().GetResult();

            var peopleStore = new EventStore<Person>(client, DATABASE, "People");
            peopleStore.Initialize().GetAwaiter().GetResult();

            var shoppingListStore = new EventStore<ShoppingList>(client, DATABASE, "ShoppingList");
            shoppingListStore.Initialize().GetAwaiter().GetResult();

            var snapshots = new SnapshotStore(client.GetDatabase(DATABASE));

            SeedData.SeedLookupsContainer(client);
            SeedData.SeedPeopleStore(peopleStore);
            

            services.AddSingleton(snapshots);

            services.AddSingleton<IEventStore<Bbq>>(bbqStore);
            services.AddSingleton<IEventStore<Person>>(peopleStore);
            services.AddSingleton<IEventStore<ShoppingList>>(shoppingListStore);
        }
    }
}

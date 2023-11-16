using Domain.Entities;
using Domain.Events;
using Eveneum;
using Microsoft.Azure.Cosmos;

namespace Infra
{
    public static class SeedData
    {
        private const string DATABASE = "Churras";

        public static void SeedLookupsContainer(CosmosClient client)
        {
            client
                .GetDatabase(DATABASE)
                .GetContainer("Lookups")
                .UpsertItemAsync(new Lookups
                {
                    PeopleIds = Data.People.Select(o => o.Id).ToList(),
                    ModeratorIds = Data.People.Where(p => p.IsCoOwner).Select(o => o.Id).ToList()
                })
                .GetAwaiter()
                .GetResult();
        }

        public static void SeedPeopleStore(EventStore<Person> peopleStore)
        {
            try
            {
                foreach (var person in Data.People)
                {
                    peopleStore
                        .WriteToStream(person.Id, new[] { new EventData(person.Id, new PersonHasBeenCreated(person.Id, person.Name, person.IsCoOwner), null, 0, DateTime.Now.ToString()) })
                        .GetAwaiter()
                        .GetResult();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("skipping already included data.");
            }
        }

        public static class Data
        {
            public static List<Person> People => new List<Person>
            {
                new Person { Id = "e5c7c990-7d75-4445-b5a2-700df354a6a0", Name = "João da Silva", IsCoOwner = false },
                new Person { Id = "171f9858-ddb1-4adf-886b-2ea36e0f0644", Name = "Marcos Oliveira", IsCoOwner = true },
                new Person { Id = "3f74e6bd-11b2-4d48-a294-239a7a2ce7d5", Name = "Gustavo Sanfoninha", IsCoOwner = true },
                new Person { Id = "795fc8f2-1473-4f19-b33e-ade1a42ed123", Name = "Alexandre Morales", IsCoOwner = false },
                new Person { Id = "addd0967-6e16-4328-bab1-eec63bf31968", Name = "Leandro Espera", IsCoOwner = false }
            };
        }
    }
}

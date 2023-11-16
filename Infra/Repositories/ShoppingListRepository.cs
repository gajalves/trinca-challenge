using Domain.Entities;
using Eveneum;

namespace Infra.Repositories
{
    internal class ShoppingListRepository : StreamRepository<ShoppingList>, IShoppingListRepository
    {
        public ShoppingListRepository(IEventStore<ShoppingList> eventStore) : base(eventStore)
        {
        }
    }
}

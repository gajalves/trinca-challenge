using Domain.Entities;

namespace Infra.Repositories
{
    public interface IShoppingListRepository : IStreamRepository<ShoppingList>
    {
    }
}

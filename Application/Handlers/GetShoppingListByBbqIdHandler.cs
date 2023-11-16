using Application.Queries;
using Application.Responses;
using Infra.Repositories;
using MediatR;

namespace Application.Handlers
{
    public class GetShoppingListByBbqIdHandler : IRequestHandler<GetShoppingListByBbqIdQuery, ShoppingListResponse>
    {
        private readonly IShoppingListRepository _shoppingListRepository;
        private readonly IPersonRepository _personRepository;

        public GetShoppingListByBbqIdHandler(IShoppingListRepository shoppingListRepository, IPersonRepository personRepository)
        {
            _shoppingListRepository = shoppingListRepository;
            _personRepository = personRepository;
        }

        public async Task<ShoppingListResponse> Handle(GetShoppingListByBbqIdQuery request, CancellationToken cancellationToken)
        {
            var person = await _personRepository.GetAsync(request.PersonId);
            if (!person.IsCoOwner)
                throw new ApplicationException("User must be co-owner to use this route");

            var list = await _shoppingListRepository.GetAsync(request.BbqId);
            
            return new ShoppingListResponse(list.Id, list.MeatQuantity, list.VegetablesQuantity);
        }
    }
}

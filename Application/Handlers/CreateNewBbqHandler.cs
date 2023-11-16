using Application.Commands;
using Application.Responses;
using CrossCutting;
using Domain.Entities;
using Domain.Events;
using Infra.Repositories;
using MediatR;

namespace Application.Handlers
{
    public class CreateNewBbqHandler : IRequestHandler<CreateNewBbqCommand, BbqResponse>
    {
        private readonly IBbqRepository _bbqRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IShoppingListRepository _shoppingListRepository;
        
        private readonly SnapshotStore _snapshots;

        public CreateNewBbqHandler(IBbqRepository bbqRepository, IPersonRepository personRepository, SnapshotStore snapshots, 
                                  IShoppingListRepository shoppingListRepository)
        {
            _bbqRepository = bbqRepository;
            _personRepository = personRepository;
            _snapshots = snapshots;
            _shoppingListRepository = shoppingListRepository;
        }

        public async Task<BbqResponse> Handle(CreateNewBbqCommand request, CancellationToken cancellationToken)
        {
            var churras = new Bbq(request.Date, request.Reason, request.IsTrincasPaying);
            
            await _bbqRepository.SaveAsync(churras);

            var shoppingList = new ShoppingList();
            shoppingList.Apply(new ShoppingListHasBeenCreated(churras.Id, 0, 0 ));
            await _shoppingListRepository.SaveAsync(shoppingList);

            var Lookups = await _snapshots.AsQueryable<Lookups>("Lookups").SingleOrDefaultAsync();

            foreach (var personId in Lookups.ModeratorIds)
            {
                var person = await _personRepository.GetAsync(personId);                 
                var @event = new PersonHasBeenInvitedToBbq(churras.Id, churras.Date, churras.Reason);
                person.Apply(@event);
                await _personRepository.SaveAsync(person);
            }

            return new BbqResponse
            {
                Id = churras.Id,
                Date = churras.Date,
                IsTrincasPaying = churras.IsTrincasPaying,
                Status = Enum.GetName(churras.Status)
            };
        }
    }
}

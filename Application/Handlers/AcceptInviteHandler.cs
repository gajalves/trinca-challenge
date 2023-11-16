using Application.Commands;
using Application.Responses;
using Azure.Core;
using Domain;
using Domain.Entities;
using Domain.Events;
using Infra.Repositories;
using MediatR;

namespace Application.Handlers
{
    public class AcceptInviteHandler : IRequestHandler<AcceptInviteCommand, PersonResponse>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IBbqRepository _bbqRepository;
        private readonly IShoppingListRepository _shoppingListRepository;

        public AcceptInviteHandler(IPersonRepository personRepository, IBbqRepository bbqRepository, IShoppingListRepository shoppingListRepository)
        {
            _personRepository = personRepository;
            _bbqRepository = bbqRepository;
            _shoppingListRepository = shoppingListRepository;
        }

        public async Task<PersonResponse> Handle(AcceptInviteCommand request, CancellationToken cancellationToken)
        {
            var @event = new InviteWasAccepted(request.PersonId, request.InviteId, request.IsVeg);

            var person = await HandlePerson(@event);
            
            await HandleChurras(@event);

            await HandleShoppingList(@event);                                               
                                                     
            return new PersonResponse
            {
                Person = person.TakeSnapshot()
            };
        }

        private async Task<Person> HandlePerson(InviteWasAccepted @event)
        {
            var person = await _personRepository.GetAsync(@event.PersonId);
            person.Apply(@event);

            await _personRepository.SaveAsync(person);

            return person;
        }

        private async Task HandleChurras(InviteWasAccepted @event)
        {
            var churras = await _bbqRepository.GetAsync(@event.InviteId);
            churras.Apply(@event);
            
            await _bbqRepository.SaveAsync(churras);
            
            await CheckIfTheBbqShouldBeConfirmed(@event.InviteId);
        }

        private async Task CheckIfTheBbqShouldBeConfirmed(string churrasId)
        {
            var churras = await _bbqRepository.GetAsync(churrasId);
            if (churras.ConfirmedPeopleId.Count == 2 && churras.Status != BbqStatus.Confirmed)
            {                
                churras.Apply(new BbqWasConfirmed(BbqStatus.Confirmed));
                await _bbqRepository.SaveAsync(churras);
            }
        }

        private async Task HandleShoppingList(InviteWasAccepted @event)
        {
            var list = await _shoppingListRepository.GetAsync(@event.InviteId);

            list.Apply(new ShoppingListQuantityAdded(@event.IsVeg, @event.PersonId));

            await _shoppingListRepository.SaveAsync(list);
        }
    }
}

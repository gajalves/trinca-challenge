using Application.Commands;
using Application.Responses;
using Azure.Core;
using Domain.Entities;
using Domain.Events;
using Infra.Repositories;
using MediatR;

namespace Application.Handlers
{
    public class DeclineInviteHandler : IRequestHandler<DeclineInviteCommand, PersonResponse>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IShoppingListRepository _shoppingListRepository;
        private readonly IBbqRepository _bbqRepository;

        public DeclineInviteHandler(IPersonRepository personRepository, IShoppingListRepository shoppingListRepository, IBbqRepository bbqRepository)
        {
            _personRepository = personRepository;
            _shoppingListRepository = shoppingListRepository;
            _bbqRepository = bbqRepository;
        }

        public async Task<PersonResponse> Handle(DeclineInviteCommand request, CancellationToken cancellationToken)
        {
            var person = await HandlePerson(request.PersonId, request.InviteId);

            if (CheckIfTheQuantityOfFoodShouldBeUpdated(person, request.InviteId))
                await UpdateFoodQuantity(request.InviteId, request.PersonId, request.IsVeg);

            await HandleBbq(request.InviteId, request.PersonId);
            await CheckIfTheBbqStatusShouldBeUpdated(request.InviteId);

            return new PersonResponse
            {
                Person = person.TakeSnapshot()
            };
        }

        private async Task HandleBbq(string inviteId, string personId)
        {
            var churras = await _bbqRepository.GetAsync(inviteId);
            churras.Apply(new InviteWasDeclined(inviteId, personId));

            await _bbqRepository.SaveAsync(churras);
        }

        private async Task<Person> HandlePerson(string personId, string inviteId)
        {
            var person = await _personRepository.GetAsync(personId);

            if (person == null)
                throw new ApplicationException("person not found");

            person.Apply(new InviteWasDeclined(inviteId, person.Id));

            await _personRepository.SaveAsync(person);

            return person;
        } 

        private async Task UpdateFoodQuantity(string inviteId, string personId, bool isVeg)
        {
            var list = await _shoppingListRepository.GetAsync(inviteId);
            list.Apply(new ShoppingListQuantityRemoved(isVeg, personId));

            await _shoppingListRepository.SaveAsync(list);
        }

        private bool CheckIfTheQuantityOfFoodShouldBeUpdated(Person person, string inviteId)
        {
            return person.Invites.Any(invite => invite.Id == inviteId);
        }

        private async Task CheckIfTheBbqStatusShouldBeUpdated(string churrasId)
        {
            var churras = await _bbqRepository.GetAsync(churrasId);
            if (churras.ConfirmedPeopleId.Count < 7 && churras.Status == BbqStatus.Confirmed)
            {
                churras.Apply(new BbqStatusUpdated(true,churras.IsTrincasPaying));
                await _bbqRepository.SaveAsync(churras);
            }
        }
    }
}

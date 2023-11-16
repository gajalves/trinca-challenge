using Application.Commands;
using Application.Responses;
using CrossCutting;
using Domain.Entities;
using Domain.Events;
using Eveneum;
using Infra.Repositories;
using MediatR;

namespace Application.Handlers
{
    public class ModerateBbqHandler : IRequestHandler<ModerateBbqCommand, BbqResponse>
    {
        private readonly IPersonRepository _persons;
        private readonly IBbqRepository _repository;
        private readonly SnapshotStore _snapshots;

        public ModerateBbqHandler(IPersonRepository persons, IBbqRepository repository, SnapshotStore snapshots)
        {
            _persons = persons;
            _repository = repository;
            _snapshots = snapshots;
        }

        public async Task<BbqResponse> Handle(ModerateBbqCommand request, CancellationToken cancellationToken)
        {
            var churras = await _repository.GetAsync(request.ChurrascoId);

            if(churras == null)
            {
                throw new ApplicationException("There is no data with provided id");
            }

            churras.Apply(new BbqStatusUpdated(request.GonnaHappen, request.TrincaWillPay));
            
            await _repository.SaveAsync(churras);

            var lookups = await _snapshots.AsQueryable<Lookups>("Lookups").SingleOrDefaultAsync();

            if (!request.GonnaHappen)
                RejectPendingInvites(churras.Id, lookups.ModeratorIds);

            if (request.GonnaHappen)
            {
                var peopleIds = RemoveModeratorIdFromPeopleIdList(lookups);

                foreach (var personId in peopleIds)
                {
                    var person = await _persons.GetAsync(personId);
                    var @event = new PersonHasBeenInvitedToBbq(churras.Id, churras.Date, churras.Reason);
                    person.Apply(@event);
                    await _persons.SaveAsync(person);
                }
            }
            
            return new BbqResponse
            {
                Id = churras.Id,
                Date = churras.Date,
                IsTrincasPaying = churras.IsTrincasPaying,
                Status = Enum.GetName(churras.Status)
            };
        }

        private async void RejectPendingInvites(string bbqId, List<string> moderatorIds)
        {
            foreach (var moderatorId in moderatorIds)
            {
                var person = await _persons.GetAsync(moderatorId);
                if(!person.Invites.Any())
                    continue; 

                var inviteId = person.Invites.Where(invite => invite.Bbq == bbqId).Select(invite => invite.Id).FirstOrDefault();
                var @event = new InviteWasDeclined(inviteId, person.Id);
                person.Apply(@event);
                await _persons.SaveAsync(person);
            }
        }        

        private List<string> RemoveModeratorIdFromPeopleIdList(Lookups lookups)
        {
            return lookups.PeopleIds.Except(lookups.ModeratorIds).ToList();
            //return lookups.PeopleIds.Where(id => !lookups.ModeratorIds.Contains(id)).ToList();
        }
    }
}

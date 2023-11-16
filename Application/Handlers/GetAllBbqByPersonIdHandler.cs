using Application.Queries;
using Application.Responses;
using Domain.Entities;
using Infra.Repositories;
using MediatR;

namespace Application.Handlers
{
    public class GetAllBbqByPersonIdHandler : IRequestHandler<GetAllBbqByPersonIdQuery, List<object>>
    {
        private readonly IBbqRepository _bbqRepository;
        private readonly IPersonRepository _personRepository;

        public GetAllBbqByPersonIdHandler(IBbqRepository bbqRepository, IPersonRepository personRepository)
        {
            _bbqRepository = bbqRepository;
            _personRepository = personRepository;
        }

        public async Task<List<object>> Handle(GetAllBbqByPersonIdQuery request, CancellationToken cancellationToken)
        {
            var person = await _personRepository.GetAsync(request.PersonId);
            var personSnapshot = person.TakeSnapshot();
            var snapshots = new List<object>();

            var bbqIds = person.Invites
                               .Where(i => i.Date > DateTime.Now && (!request.IncludeRejectedBbq && i.Status != InviteStatus.Declined))
                               .Select(o => o.Id)
                               .ToList();

            foreach (var bbqId in bbqIds)
            {
                var bbq = await _bbqRepository.GetAsync(bbqId);
                var bbqSnapShot = bbq.TakeSnapshot();

                snapshots.Add(bbq.TakeSnapshot());
            }            

            return snapshots;
        }
    }
}

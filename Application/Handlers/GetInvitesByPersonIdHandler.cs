using Application.Queries;
using Domain.Entities;
using Infra.Repositories;
using MediatR;

namespace Application.Handlers
{
    public class GetInvitesByPersonIdHandler : IRequestHandler<GetInvitesByPersonIdQuery, Person>
    {
        private readonly IPersonRepository _repository;

        public GetInvitesByPersonIdHandler(IPersonRepository repository)
        {
            _repository = repository;
        }

        public async Task<Person> Handle(GetInvitesByPersonIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAsync(request.personId);            
        }
    }
}

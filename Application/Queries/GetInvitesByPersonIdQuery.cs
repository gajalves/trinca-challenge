using Domain.Entities;
using MediatR;

namespace Application.Queries
{
    public class GetInvitesByPersonIdQuery : IRequest<Person>
    {
        public string personId { get; set; }
    }
}

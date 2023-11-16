using Application.Responses;
using MediatR;

namespace Application.Commands
{
    public class AcceptInviteCommand : IRequest<PersonResponse>
    {
        public string PersonId { get; set; }
        public string InviteId { get; set; }
        public bool IsVeg { get; set; }
    }
}

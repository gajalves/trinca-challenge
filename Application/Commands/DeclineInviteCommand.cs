using Application.Responses;
using MediatR;

namespace Application.Commands
{
    public class DeclineInviteCommand : IRequest<PersonResponse>
    {
        public DeclineInviteCommand(string inviteId, string personId, bool isVeg)
        {
            InviteId = inviteId;
            PersonId = personId;
            IsVeg = isVeg;
        }

        public string InviteId { get; set; }
        public string PersonId { get; set; }
        public bool IsVeg { get; set; }
    }
}

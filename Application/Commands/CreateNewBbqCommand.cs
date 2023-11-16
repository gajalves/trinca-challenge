using Application.Responses;
using MediatR;

namespace Application.Commands
{
    public class CreateNewBbqCommand : IRequest<BbqResponse>
    {        
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public bool IsTrincasPaying { get; set; }

        public CreateNewBbqCommand(DateTime date, string reason, bool isTrincasPaying)
        {
            Date = date;
            Reason = reason;
            IsTrincasPaying = isTrincasPaying;
        }
    }
}

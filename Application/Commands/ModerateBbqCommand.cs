using Application.Responses;
using MediatR;

namespace Application.Commands
{
    public class ModerateBbqCommand : IRequest<BbqResponse>
    {
        public string ChurrascoId { get; set; }
        public bool GonnaHappen { get; set; }
        public bool TrincaWillPay { get; set; }
        
        public ModerateBbqCommand(string churrascoId, bool gonnaHappen, bool trincaWillPay)
        {
            ChurrascoId = churrascoId;
            GonnaHappen = gonnaHappen;
            TrincaWillPay = trincaWillPay;
        }
    }
}

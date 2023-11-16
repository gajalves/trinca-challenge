using Application.Commands;
using Domain.Entities;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Serverless_Api
{
    public partial class RunDeclineInvite
    {
        private readonly Person _user;        
        private readonly IMediator _mediator;

        public RunDeclineInvite(Person user, IMediator mediator)
        {
            _user = user;            
            _mediator = mediator;
        }

        [Function(nameof(RunDeclineInvite))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "put", Route = "person/invites/{inviteId}/decline")] HttpRequestData req, string inviteId)
        {

            var answer = await req.Body<DeclineInviteCommand>();

            if(answer == null)
                return await req.CreateResponse(System.Net.HttpStatusCode.BadRequest, "IsVeg cannot be null");

            var request = new DeclineInviteCommand(inviteId, _user.Id, answer.IsVeg);

            var response = await _mediator.Send(request);
                        
            return await req.CreateResponse(System.Net.HttpStatusCode.OK, response);
        }
    }
}

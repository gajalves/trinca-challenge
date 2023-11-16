using Application.Commands;
using Domain.Entities;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Serverless_Api
{
    public partial class RunAcceptInvite
    {
        private readonly Person _user;
        private readonly IMediator _mediator;

        public RunAcceptInvite(Person user, IMediator mediator)
        {
            _user = user;            
            _mediator = mediator;
        }

        [Function(nameof(RunAcceptInvite))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "put", Route = "person/invites/{inviteId}/accept")] HttpRequestData req, string inviteId)
        {
            var answer = await req.Body<AcceptInviteCommand>();
            answer.InviteId = inviteId;
            answer.PersonId = _user.Id;

            var response = await _mediator.Send(answer);
                                   
            return await req.CreateResponse(System.Net.HttpStatusCode.OK, response);
        }
    }
}

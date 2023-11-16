using Application.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Serverless_Api
{
    public partial class RunGetInvites
    {
        private readonly Person _user;
        private readonly IMediator _mediator;

        public RunGetInvites(IMediator mediator, Person user)
        {
            _mediator = mediator;
            _user = user;
        }

        [Function(nameof(RunGetInvites))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "person/invites")] HttpRequestData req)
        {            
            var query = new GetInvitesByPersonIdQuery
            {
                personId = _user.Id,
            };

            var response = await _mediator.Send(query);
            
            if (response == null)
                return req.CreateResponse(System.Net.HttpStatusCode.NoContent);

            return await req.CreateResponse(System.Net.HttpStatusCode.OK, response.TakeSnapshot());
        }
    }
}

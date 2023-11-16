using Application.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace Serverless_Api
{
    public partial class RunGetProposedBbqs
    {
        private readonly Person _user;
        private readonly IMediator _mediator;

        public RunGetProposedBbqs(IMediator mediator, Person user)
        {
            _mediator = mediator;
            _user = user;
        }

        [Function(nameof(RunGetProposedBbqs))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "churras")] HttpRequestData req)
        {            
            if(string.IsNullOrEmpty(_user.Id))
                return await req.CreateResponse(HttpStatusCode.BadRequest, "personId is required.");

            var query = new GetAllBbqByPersonIdQuery
            (
                false,
                _user.Id
            );

            var response = await _mediator.Send(query);
           
            if(response.Any())
                return await req.CreateResponse(HttpStatusCode.Accepted, response);

            return await req.CreateResponse(HttpStatusCode.NoContent, response);
        }
    }
}

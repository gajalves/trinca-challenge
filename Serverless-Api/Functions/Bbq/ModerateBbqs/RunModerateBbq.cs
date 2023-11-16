using Application.Commands;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace Serverless_Api
{
    public partial class RunModerateBbq
    {        
        private readonly IMediator _mediator;

        public RunModerateBbq(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [Function(nameof(RunModerateBbq))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "put", Route = "churras/{id}/moderar")] HttpRequestData req, string id)
        {
            var moderationRequest = await req.Body<ModerateBbqCommand>();

            if (moderationRequest == null)
            {
                return await req.CreateResponse(HttpStatusCode.BadRequest, "input is required.");
            }

            moderationRequest.ChurrascoId = id;

            var response = await _mediator.Send(moderationRequest);

            return await req.CreateResponse(HttpStatusCode.Created, response);            
        }
    }
}

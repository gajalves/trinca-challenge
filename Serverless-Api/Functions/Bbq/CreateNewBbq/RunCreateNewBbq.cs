using Application.Commands;
using Eveneum;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace Serverless_Api
{
    public partial class RunCreateNewBbq
    {        
        private readonly IMediator _mediator;

        public RunCreateNewBbq(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [Function(nameof(RunCreateNewBbq))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "churras")] HttpRequestData req)
        {
            var input = await req.Body<CreateNewBbqCommand>();

            if (input == null)
            {
                return await req.CreateResponse(HttpStatusCode.BadRequest, "input is required.");
            }

            var response = await _mediator.Send(input);
            
            return await req.CreateResponse(HttpStatusCode.Created, response);
        }

        private void teste(string id, EventData[] eventDatas, object expectedVersion)
        {
            throw new NotImplementedException();
        }
    }
}

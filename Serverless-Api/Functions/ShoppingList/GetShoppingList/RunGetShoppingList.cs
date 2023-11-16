using Application.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Serverless_Api.Functions.ShoppingList.GetShoppingList
{
    public class RunGetShoppingList
    {
        private readonly Person _user;
        private readonly IMediator _mediator;

        public RunGetShoppingList(IMediator mediator, Person user)
        {
            _mediator = mediator;
            _user = user;
        }

        [Function(nameof(RunGetShoppingList))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "shoppinglist/{bbqId}")] HttpRequestData req, string bbqId)
        {
            var query = new GetShoppingListByBbqIdQuery(_user.Id, bbqId);

            var response = await _mediator.Send(query);

            return await req.CreateResponse(System.Net.HttpStatusCode.OK, response);
        }
    }
}

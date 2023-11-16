using Application.Responses;
using MediatR;

namespace Application.Queries
{
    public class GetShoppingListByBbqIdQuery : IRequest<ShoppingListResponse>
    {
        public GetShoppingListByBbqIdQuery(string personId, string bbqId)
        {
            PersonId = personId;
            BbqId = bbqId;
        }

        public string PersonId { get; set; }
        public string BbqId { get; set; }
    }
}

using Application.Responses;
using MediatR;

namespace Application.Queries
{
    public class GetAllBbqByPersonIdQuery : IRequest<List<object>>
    {
        public string PersonId { get; set; }
        public bool IncludeRejectedBbq { get; set; }

        public GetAllBbqByPersonIdQuery(bool includeRejectedBbq, string personId)
        {
            IncludeRejectedBbq = includeRejectedBbq;
            PersonId = personId;
        }
    }
}

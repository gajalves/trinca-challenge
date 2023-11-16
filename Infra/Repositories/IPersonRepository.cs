using Domain.Entities;

namespace Infra.Repositories
{
    public interface IPersonRepository : IStreamRepository<Person>
    {

    }
}
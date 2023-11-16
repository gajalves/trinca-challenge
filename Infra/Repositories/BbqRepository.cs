using System;
using Domain.Entities;

namespace Infra.Repositories
{
    internal class BbqRepository : StreamRepository<Bbq>, IBbqRepository
    {
        public BbqRepository(IEventStore<Bbq> eventStore) : base(eventStore) { }
    }
}

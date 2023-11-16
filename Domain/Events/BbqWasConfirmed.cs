using Domain.Entities;
using System;

namespace Domain.Events
{
    public class BbqWasConfirmed : IEvent
    {        
        public BbqStatus Status { get; set; }

        public BbqWasConfirmed(BbqStatus status)
        {            
            Status = status;
        }
    }
}

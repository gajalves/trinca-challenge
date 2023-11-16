using System;
using System.Collections.Generic;
using Azure.Core;
using Domain.Events;

namespace Domain.Entities
{
    public class Bbq : AggregateRoot
    {

        public string Reason { get; set; }
        public BbqStatus Status { get; set; }
        public DateTime Date { get; set; }
        public bool IsTrincasPaying { get; set; }
        public List<string> ConfirmedPeopleId { get; set; } = new List<string>();        

        public Bbq()
        {            
        }

        public Bbq(DateTime date, string reason,  bool isTrincasPaying)
        {
            Apply(new ThereIsSomeoneElseInTheMood(Guid.NewGuid(), date, reason, isTrincasPaying));
        }

        public void When(ThereIsSomeoneElseInTheMood @event)
        {
            Id = @event.Id.ToString();
            Date = @event.Date;
            Reason = @event.Reason;
            Status = BbqStatus.New;
        }

        public void When(BbqStatusUpdated @event)
        {
            if (@event.GonnaHappen)
                Status = BbqStatus.PendingConfirmations;
            else 
                Status = BbqStatus.ItsNotGonnaHappen;

            if (@event.TrincaWillPay)
                IsTrincasPaying = true;
        }

        public void When(InviteWasDeclined @event)
        {
            ConfirmedPeopleId.Remove(@event.PersonId);
        }

        public void When(InviteWasAccepted @event)
        {
            if (ConfirmedPeopleId.Contains(@event.PersonId))
                return;

            ConfirmedPeopleId.Add(@event.PersonId);            
        }
        
        public void When(BbqWasConfirmed @event)
        {
            Status = @event.Status;
        }

        public object TakeSnapshot()
        {
            return new
            {
                Id,
                Date,
                IsTrincasPaying,
                Status = Status.ToString()
            };
        }
    }
}

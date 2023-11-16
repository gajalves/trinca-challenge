﻿namespace Domain.Events
{
    public class InviteWasDeclined : IEvent
    {
        public InviteWasDeclined(string inviteId, string personId)
        {
            InviteId = inviteId;
            PersonId = personId;
        }

        public string InviteId { get; set; }
        public string PersonId { get; set; }
    }
}

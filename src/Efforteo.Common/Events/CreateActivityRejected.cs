using System;

namespace Efforteo.Common.Events
{
    public class CreateActivityRejected : IRejectedEvent
    {
        public Guid Id { get; }
        public string Code { get; }
        public string Reason { get; }

        protected CreateActivityRejected()
        {
        }

        public CreateActivityRejected(Guid id, string code, string reason)
        {
            Id = id;
            Code = code;
            Reason = reason;
        }
    }
}
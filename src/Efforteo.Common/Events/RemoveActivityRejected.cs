using System;

namespace Efforteo.Common.Events
{
    public class RemoveActivityRejected : IRejectedEvent
    {
        public Guid Id { get; }
        public string Code { get; }
        public string Reason { get; }

        protected RemoveActivityRejected()
        {
        }

        public RemoveActivityRejected(Guid id, string code, string reason)
        {
            Id = id;
            Code = code;
            Reason = reason;
        }
    }
}
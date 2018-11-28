using System;

namespace Efforteo.Common.Events
{
    public class RemoveUserRejected : IRejectedEvent
    {
        public Guid Id { get; }
        public string Code { get; }
        public string Reason { get; }

        protected RemoveUserRejected()
        {
        }

        public RemoveUserRejected(Guid id, string code, string reason)
        {
            Id = id;
            Code = code;
            Reason = reason;
        }
    }
}

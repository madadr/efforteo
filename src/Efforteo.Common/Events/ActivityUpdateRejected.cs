using System;

namespace Efforteo.Common.Events
{
    public class ActivityUpdateRejected : IRejectedEvent
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Code { get; }
        public string Reason { get; }

        protected ActivityUpdateRejected()
        {
        }

        public ActivityUpdateRejected(Guid id, Guid userId, string code, string reason)
        {
            Id = id;
            UserId = userId;
            Code = code;
            Reason = reason;
        }
    }
}
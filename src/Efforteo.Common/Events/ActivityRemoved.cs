using System;

namespace Efforteo.Common.Events
{
    public class ActivityRemoved : IEvent
    {
        public Guid Id { get; }
        public Guid UserId { get; }

        protected ActivityRemoved()
        {

        }

        public ActivityRemoved(Guid userId, Guid id)
        {
            UserId = userId;
            Id = id;
        }
    }
}

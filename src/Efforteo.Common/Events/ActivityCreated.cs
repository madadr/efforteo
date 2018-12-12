using System;

namespace Efforteo.Common.Events
{
    public class ActivityCreated : IAuthenticatedEvent
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Category { get; }
        public string Name { get; }
        public string Description { get; }
        public long Time { get; }
        public float Distance { get; }

        protected ActivityCreated()
        {

        }

        public ActivityCreated(Guid id, Guid userId, string category, string name, string description, long time, float distance)
        {
            Id = id;
            UserId = userId;
            Category = category;
            Name = name;
            Description = description;
            Time = time;
            Distance = distance;
        }
    }
}

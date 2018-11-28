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

        protected ActivityCreated()
        {

        }

        public ActivityCreated(Guid userId, Guid id, string category, string name, string description)
        {
            UserId = userId;
            Id = id;
            Category = category;
            Name = name;
            Description = description;
        }
    }
}

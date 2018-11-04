using System;
using Efforteo.Common.Events;
using IAuthenticatedEvent = Efforteo.Common.Commands.IAuthenticatedEvent;

namespace Efforteo.Services.Activities.Domain.Models
{
    public class Activity
    {
        public Guid UserId { get; protected set; }
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string Category { set; protected get; }
        public string Description { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        protected Activity()
        {
        }

        public Activity(Guid userId, Guid id, string name, Category category, string description, DateTime createdAt)
        {
            UserId = userId;
            Id = id;
            Name = name;
            Category = category.Name;
            Description = description;
            CreatedAt = createdAt;
        }
    }
}

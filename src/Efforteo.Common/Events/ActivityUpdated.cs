﻿using System;

namespace Efforteo.Common.Events
{
    public class ActivityUpdated : IAuthenticatedEvent
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Title { get; }
        public string Category { get; }
        public string Description { get; }
        public long Time { get; }
        public float Distance { get; }

        protected ActivityUpdated()
        {

        }

        public ActivityUpdated(Guid id, Guid userId, string category, string title, string description, long time, float distance)
        {
            Id = id;
            UserId = userId;
            Category = category;
            Title = title;
            Description = description;
            Time = time;
            Distance = distance;
        }
    }
}
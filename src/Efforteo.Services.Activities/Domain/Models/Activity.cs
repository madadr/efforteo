using System;
using Efforteo.Common.Exceptions;

namespace Efforteo.Services.Activities.Domain.Models
{
    public class Activity
    {
        public Guid UserId { get; protected set; }
        public Guid Id { get; protected set; }
        public string Title { get; protected set; }
        public string Category { set; protected get; }
        public string Description { get; protected set; }
        public long Time { get; protected set; }
        public float Distance { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        protected Activity()
        {
        }

        public Activity(Guid userId, Guid id, string title, Category category, string description, long time,
            float distance, DateTime createdAt)
        {
            UserId = userId;
            Id = id;

            SetTitle(title);
            SetCategory(category.Name);
            SetDescription(description);
            SetTime(time);
            SetDistance(distance);
            SetCreatedAt(createdAt);
        }

        private void SetCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new EfforteoException("empty_category", "Category cannot be empty");
            }

            Category = category;
        }

        private void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new EfforteoException("empty_title", "Title cannot be empty");
            }

            Title = title;
        }

        private void SetDescription(string description)
        {
            Description = description ?? "";
        }

        private void SetTime(long time)
        {
            if (time < 0)
            {
                throw new EfforteoException("invalid_time", "Time cannot be negative");
            }

            Time = time;
        }

        private void SetDistance(float distance)
        {
            if (distance < 0)
            {
                throw new EfforteoException("invalid_distance", "Distance cannot be negative");
            }

            Distance = distance;
        }

        private void SetCreatedAt(DateTime createdAt)
        {
            CreatedAt = createdAt;

//            if (CreatedAt > DateTime.UtcNow.AddMinutes(1))
//            {
//                CreatedAt = DateTime.UtcNow;
//            }
        }

        public void SetData(string title, string category, string description, long? time, float? distance,
            DateTime? createdAt)
        {
            if (title != null) SetTitle(title);
            if (category != null) SetCategory(category);
            if (description != null) SetDescription(description);
            if (time != null) SetTime(time.Value);
            if (distance != null) SetDistance(distance.Value);
            if (createdAt != null) SetCreatedAt(createdAt.GetValueOrDefault(DateTime.UtcNow));
        }
    }
}
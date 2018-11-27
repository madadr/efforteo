using System;
using Efforteo.Common.Exceptions;

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

            SetName(name);
            SetCategory(name);
            SetDescription(name);

            CreatedAt = DateTime.UtcNow;
        }

        private void SetCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new EfforteoException("empty_category", "Category cannot be empty");
            }

            Category = category;
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new EfforteoException("empty_name", "Name cannot be empty");
            }

            Name = name;
        }

        private void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new EfforteoException("empty_description", "Description cannot be empty");
            }

            Description = description;
        }

        public void SetData(string name, string category, string description)
        {
            if(name != null) SetName(name);
            if(category != null) SetCategory(category);
            if(description != null) SetDescription(description);
        }
    }
}
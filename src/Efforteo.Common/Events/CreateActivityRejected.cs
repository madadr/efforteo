using System;
using System.Collections.Generic;
using System.Text;

namespace Efforteo.Common.Events
{
    public class CreateActivityRejected : IRejectedEvent
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Reason { get; }
        public string Code { get; }

        protected CreateActivityRejected()
        {
        }

        public CreateActivityRejected(Guid id, Guid userId, string category, string name, string description, string reason, string code)
        {
            Id = id;
            UserId = userId;
            Category = category;
            Name = name;
            Description = description;
            Reason = reason;
            Code = code;
        }
    }
}

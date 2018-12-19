using System;

namespace Efforteo.Common.Events
{
    public class UserCreated : IEvent
    {
        public Guid Id { get; }
        public string Email { get; }
        public string Name { get; }

        protected UserCreated()
        {
        }

        public UserCreated(Guid id, string email, string name)
        {
            Id = id;
            Email = email;
            Name = name;
        }
    }
}
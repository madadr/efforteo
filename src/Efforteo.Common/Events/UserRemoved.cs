using System;

namespace Efforteo.Common.Events
{
    public class UserRemoved : IEvent
    {
        public Guid Id { get; }
        public string Email { get; }

        protected UserRemoved()
        {
        }

        public UserRemoved(Guid id, string email)
        {
            Id = id;
            Email = email;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Efforteo.Common.Events
{
    public class PasswordChanged : IAuthenticatedEvent
    {
        public Guid UserId { get; }

        public PasswordChanged()
        {
        }

        public PasswordChanged(Guid userId)
        {
            UserId = userId;
        }
    }
}
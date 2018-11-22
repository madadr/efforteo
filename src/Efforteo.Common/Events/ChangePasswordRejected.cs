using System;
using System.Collections.Generic;
using System.Text;

namespace Efforteo.Common.Events
{
    public class ChangePasswordRejected : IRejectedEvent
    {
        public Guid UserId { get; }
        public string Code { get; }
        public string Reason { get; }

        public ChangePasswordRejected()
        {
        }

        public ChangePasswordRejected(Guid userId, string code, string reason)
        {
            UserId = userId;
            Code = code;
            Reason = reason;
        }
    }
}

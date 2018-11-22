using System;
using System.Collections.Generic;
using System.Text;

namespace Efforteo.Common.Commands
{
    public class ChangePassword : IAuthenticatedCommand
    {
        public Guid UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}

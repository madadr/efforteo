using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Efforteo.Common.Commands
{
    public class RemoveUser : IAuthenticatedCommand
    {
        public Guid UserId { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Efforteo.Common.Commands
{
    public class UpdateAccount : IAuthenticatedCommand
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public float Weight { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
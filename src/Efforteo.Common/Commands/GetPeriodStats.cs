using System;

namespace Efforteo.Common.Commands
{
    public class GetPeriodStats : IAuthenticatedCommand
    {
        public Guid UserId { get; set; }
        public int Days { get; set; }
    }
}

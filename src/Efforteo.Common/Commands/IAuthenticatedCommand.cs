using System;
using System.Collections.Generic;
using System.Text;

namespace Efforteo.Common.Commands
{
    public interface IAuthenticatedCommand : ICommand
    {
        Guid UserId { get; set; }
    }
}
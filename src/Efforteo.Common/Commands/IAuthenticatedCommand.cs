using System;

namespace Efforteo.Common.Commands
{
    public interface IAuthenticatedEvent : ICommand
    {
        Guid UserId { get; set; }
    }
}
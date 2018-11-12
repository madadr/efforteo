using System;

namespace Efforteo.Common.Auth
{
    public interface IJwtHandler
    {
        JsonWebToken Create(Guid userId);
    }
}
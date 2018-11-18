using System;
using Efforteo.Common.Exceptions;
using Efforteo.Services.Accounts.Domain.Services;

namespace Efforteo.Services.Accounts.Domain.DTO
{
    public class UserDto
    {
        public Guid Id { get;  set; }
        public string Email { get;  set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
using System;
using Efforteo.Common.Exceptions;
using Efforteo.Services.Authentication.Domain.Services;

namespace Efforteo.Services.Authentication.Domain.DTO
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
using System;
using Efforteo.Common.Exceptions;

namespace Efforteo.Services.Accounts.Domain.DTO
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public float Weight { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastLoggedIn { get; set; }
    }
}
using System;
using Efforteo.Common.Exceptions;
using Efforteo.Services.Authentication.Domain.Services;

namespace Efforteo.Services.Authentication.Domain.Models
{
    public class User
    {
        public Guid Id { get; protected set; }
        public string Email { get; protected set; }
        public string Password { get; protected set; }
        public string Salt { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        public User(string email, string name)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new EfforteoException("empty_email", "User e-mail cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new EfforteoException("empty_name", "User name cannot be empty");
            }

            Id = Guid.NewGuid();
            Email = email.ToLowerInvariant();
            CreatedAt = DateTime.UtcNow;
        }

        public void SetPassword(string password, IEncrypter encrypter)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new EfforteoException("empty_password", "Password can not be empty.");
            }
            Salt = encrypter.GetSalt();
            Password = encrypter.GetHash(password, Salt);
        }

        public bool ValidatePassword(string password, IEncrypter encrypter)
            => Password.Equals(encrypter.GetHash(password, Salt));
    }
}
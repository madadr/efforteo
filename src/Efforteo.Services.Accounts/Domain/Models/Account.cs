using System;
using Efforteo.Common.Exceptions;

namespace Efforteo.Services.Accounts.Domain.Models
{
    public class Account
    {
        public Guid Id { get; protected set; }
        public string Email { get; protected set; }
        public string Name { get; protected set; }
        public string Location { get; protected set; }
        public float Weight { get; protected set; }
        public DateTime? Birthday { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime LastLoggedIn { get; protected set; }

        public Account()
        {
        }

        public Account(Guid id, string email)
        {
            SetId(id);
            SetEmail(email);

            CreatedAt = DateTime.UtcNow;
            LastLoggedIn = DateTime.UtcNow;
        }

        public Account(Guid id, string email, string name, string location, float weight, DateTime? birthday) : this(id,
            email)
        {
            SetName(name);
            SetLocation(location);
            SetWeight(weight);
            SetBirthday(birthday);
        }

        public Account(Guid id, string email, string name, string location, float weight, DateTime birthday,
            DateTime createdAt, DateTime lastLoggedIn) : this(id, email, name, location, weight, birthday)
        {
            CreatedAt = createdAt;
            LastLoggedIn = lastLoggedIn;
        }

        private void SetId(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new EfforteoException("empty_id", "Account id cannot be empty");
            }

            Id = id;
        }

        private void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new EfforteoException("empty_email", "Account e-mail cannot be empty");
            }

            Email = email.ToLowerInvariant();
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new EfforteoException("empty_firstName", "First name cannot be empty");
            }

            Name = name;
        }

        private void SetLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                throw new EfforteoException("empty_country", "Country cannot be empty");
            }

            Location = location;
        }

        private void SetBirthday(DateTime? birthday)
        {
            if (birthday.HasValue && birthday.Value.CompareTo(DateTime.UtcNow) >= 0)
            {
                throw new EfforteoException("invalid_birthday", "Birthday date cannot be earlier than today");
            }

            Birthday = birthday;
        }

        private void SetWeight(float weight)
        {
            if (weight < 0 || weight > 400)
            {
                throw new EfforteoException("invalid_weight", "Weight is not valid");
            }

            Weight = weight;
        }

        public void SetAccountData(string email, string name, string location, float weight, DateTime? birthday)
        {
            if (email != null) SetEmail(email);
            if (name != null) SetName(name);
            if (location != null) SetLocation(location);
            SetWeight(weight);
            if (birthday.HasValue) SetBirthday(birthday);
        }

        public void UpdateLastLoggedIn()
        {
            LastLoggedIn = DateTime.UtcNow;
        }
    }
}
using System;
using Efforteo.Common.Exceptions;

namespace Efforteo.Services.Stats.Domain.Models
{
    public class Stat
    {
        public Guid Id { get; protected set; }
        public Guid UserId { get; protected set; }
        public string Category { get; protected set; }
        public float Distance { get; protected set; }
        public long Time { get; protected set; }
        public float? Speed { get; protected set; }
        public int? Pace { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        public Stat(Guid id, Guid userId, string category, float distance, long time, DateTime createdAt)
        {
            SetId(id);
            SetUserId(userId);
            Category = category;
            Distance = distance;
            Time = time;
            CreatedAt = createdAt;

            CalculateSpeed();
            CalculatePace();
        }

        private void SetId(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new EfforteoException("empty_id", "Stat id cannot be empty");
            }

            Id = id;
        }

        private void SetUserId(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new Exception("empty_id");
            }

            UserId = userId;
        }

        private void CalculateSpeed()
        {
            if (Time == 0 || Math.Abs(Distance) <= 0.001)
            {
                Speed = null;
            }
            else
            {
                Speed = (Distance / Time) * 3600;
            }
        }

        private void CalculatePace()
        {
            if (Time == 0 || Math.Abs(Distance) <= 0.001)
            {
                Pace = null;
            }
            else
            {
                Pace = (int) (Time / Distance);
            }
        }

        public void SetStatData(string category, float distance, long time)
        {
            Category = category;
            Distance = distance;
            Time = time;

            CalculateSpeed();
            CalculatePace();
        }
    }
}
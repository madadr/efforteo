using System;
using Efforteo.Common.Exceptions;

namespace Efforteo.Services.Stats.Domain.DTO
{
    public class StatDto
    {
        public Guid Id { get; set; }
        public string Category { get; set; }
        public float Distance { get; set; }
        public long Time { get; set; }
        public float? Speed { get; set; }
        public int? Pace { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
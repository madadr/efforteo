using System;

namespace Efforteo.Services.Activities.Domain.DTO
{
    public class ActivityDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Category { set; get; }
        public string Description { get; set; }
        public long? Time { get; set; }
        public float? Distance { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
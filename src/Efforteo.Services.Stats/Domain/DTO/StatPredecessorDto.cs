using System;
using Efforteo.Common.Exceptions;

namespace Efforteo.Services.Stats.Domain.DTO
{
    public class StatPredecessorDto
    {
        public Guid Id { get; set; }
        public float DeltaSpeed { get; set; }
        public int DeltaPace { get; set; }
    }
}
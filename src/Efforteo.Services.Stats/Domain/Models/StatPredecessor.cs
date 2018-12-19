using System;
using Efforteo.Common.Exceptions;

namespace Efforteo.Services.Stats.Domain.Models
{
    public class StatPredecessor
    {
        public Guid Id { get; protected set; }
        public float DeltaSpeed { get; protected set; }
        public int DeltaPace { get; protected set; }

        public StatPredecessor()
        {
        }

        public StatPredecessor(Guid id, float deltaSpeed, int deltaPace)
        {
            Id = id;
            DeltaSpeed = deltaSpeed;
            DeltaPace = deltaPace;
        }
    }
}
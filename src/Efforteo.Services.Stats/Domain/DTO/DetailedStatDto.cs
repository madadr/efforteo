using System;
using Efforteo.Common.Exceptions;

namespace Efforteo.Services.Stats.Domain.DTO
{
    public class DetailedStatDto
    {
        public StatDto Stat { get; set; }
        public StatPredecessorDto Predecessor { get; set; }
    }
}
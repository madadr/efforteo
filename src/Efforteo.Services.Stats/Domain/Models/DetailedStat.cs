using System;
using Efforteo.Common.Exceptions;

namespace Efforteo.Services.Stats.Domain.Models
{
    public class DetailedStat
    {
        public Stat Stat { get; set; }
        public StatPredecessor Predecessor { get; set; }

        public DetailedStat()
        {
        }

        public DetailedStat(Stat stat, StatPredecessor predecessor)
        {
            Stat = stat;
            Predecessor = predecessor;
        }
    }
}
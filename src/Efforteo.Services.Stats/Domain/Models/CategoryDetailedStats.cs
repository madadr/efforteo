using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Efforteo.Services.Stats.Domain.DTO;

namespace Efforteo.Services.Stats.Domain.Models
{
    public class CategoryDetailedStats
    {
        public string Category { get; protected set; }
        public DetailedStat[] Stats { get; protected set; }

        public CategoryDetailedStats(IEnumerable<Stat> stats)
        {
            if (!stats.Any())
            {
                return;
            }

            Category = stats.First().Category.ToLowerInvariant();

            stats = stats.OrderByDescending(stat => stat.CreatedAt);
            Stats = new DetailedStat[stats.Count()];

            CalculateDetailedStats(stats);
        }

        private void CalculateDetailedStats(IEnumerable<Stat> stats)
        {
            for (int i = 0; i < stats.Count(); i++)
            {
                DetailedStat stat = new DetailedStat();
                stat.Stat = stats.ElementAt(i);

                if (stat.Stat.Pace.HasValue && stat.Stat.Speed.HasValue)
                {
                    for (int j = i + 1; j < stats.Count(); ++j)
                    {
                        var st = stats.ElementAt(j);
                        if (!st.Pace.HasValue || !st.Speed.HasValue)
                        {
                            continue;
                        }

                        stat.Predecessor = CreatePredecessor(stat.Stat, st);
                        break;
                    }
                }

                Stats[i] = stat;
            }
        }

        private StatPredecessor CreatePredecessor(Stat first, Stat second)
            => new StatPredecessor(second.Id, second.Speed.Value - first.Speed.Value,
                first.Pace.Value - second.Pace.Value);
    }
}
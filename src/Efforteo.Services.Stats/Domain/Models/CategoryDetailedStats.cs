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
        public DetailedStatDto[] Stats { get; protected set; }

        public CategoryDetailedStats(IEnumerable<Stat> stats, IMapper mapper)
        {
            if (!stats.Any())
            {
                return;
            }

            Category = stats.First().Category.ToLowerInvariant();

            stats = stats.OrderByDescending(stat => stat.CreatedAt);
            Stats = new DetailedStatDto[stats.Count()];

            CalculateDetailedStats(stats, mapper);
        }

        private void CalculateDetailedStats(IEnumerable<Stat> stats, IMapper mapper)
        {
            for (int i = 0; i < stats.Count(); i++)
            {
                DetailedStatDto stat = new DetailedStatDto();
                stat.Stat = mapper.Map<StatDto>(stats.ElementAt(i));

                if (stat.Stat.Pace.HasValue && stat.Stat.Speed.HasValue)
                {
                    for (int j = i + 1; j < stats.Count(); ++j)
                    {
                        var st = stats.ElementAt(j);
                        if (!st.Pace.HasValue || !st.Speed.HasValue)
                        {
                            continue;
                        }

                        stat.Predecessor = CreatePredecessor(stat.Stat, mapper.Map<StatDto>(st));
                        break;
                    }
                }

                Stats[i] = stat;
            }
        }

        private StatPredecessorDto CreatePredecessor(StatDto first, StatDto second)
            => new StatPredecessorDto
            {
                Id = second.Id,
                DeltaPace = first.Pace.Value - second.Pace.Value,
                DeltaSpeed = first.Speed.Value - second.Speed.Value
            };
    }
}
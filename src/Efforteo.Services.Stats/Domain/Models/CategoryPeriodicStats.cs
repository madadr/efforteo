using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Efforteo.Common.Exceptions;

namespace Efforteo.Services.Stats.Domain.Models
{
    public class CategoryPeriodicStats
    {
        public string Category { get; protected set; }
        public int Days { get; protected set; }
        public DateTime PeriodStart { get; protected set; }
        public float[] Distance { get; protected set; }
        public long[] Time { get; protected set; }

        public CategoryPeriodicStats(IEnumerable<Stat> stats, int days)
        {
            if (!stats.Any())
            {
                return;
            }

            Category = stats.First().Category.ToLowerInvariant();
            Days = days;
            var day = DateTime.UtcNow.AddDays(1 - Days);
            PeriodStart = new DateTime(day.Year, day.Month, day.Day, 0, 0, 0, DateTimeKind.Utc);
            Distance = new float[Days];
            Time = new long[Days];
            CalculateMeasures(stats);
        }

        private void SetCategory(IEnumerable<Stat> stats)
        {
        }

        private void CalculateMeasures(IEnumerable<Stat> stats)
        {
            for (int i = 0; i < Days; ++i)
            {
                var dayStats = stats.Where(stat =>
                    stat.CreatedAt >= PeriodStart.AddDays(i) && stat.CreatedAt <= PeriodStart.AddDays(i + 1));
                if (dayStats.Any())
                {
                    Distance[i] = dayStats.Sum(stat => stat.Distance);
                    Time[i] = dayStats.Sum(stat => stat.Time);
                }
            }
        }
    }
}
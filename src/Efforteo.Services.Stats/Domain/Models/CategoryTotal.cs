using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Efforteo.Services.Stats.Domain.DTO;

namespace Efforteo.Services.Stats.Domain.Models
{
    public class CategoryTotal
    {
        public string Category { get; protected set; }
        public int Amount { get; protected set; }
        public long Time { get; protected set; }
        public float Distance { get; protected set; }
        public float? AverageSpeed { get; protected set; }
        public int? AveragePace { get; protected set; }
        public ActivityPointer LongestDistance { get; protected set; }
        public ActivityPointer LongestTime { get; protected set; }

        public CategoryTotal(IEnumerable<Stat> stats)
        {
            if (!stats.Any())
            {
                return;
            }

            Category = stats.First().Category;
            Amount = stats.Count();
            Time = stats.Sum(s => s.Time);
            Distance = stats.Sum(s => s.Distance);
            CalculateSpeed();
            CalculatePace();
            CalculateLongestDistance(stats);
            CalculateLongestTime(stats);
        }
        private void CalculateSpeed()
        {
            if (Time == 0 || Math.Abs(Distance) <= 0.001)
            {
                AverageSpeed = null;
            }
            else
            {
                AverageSpeed = (Distance / Time) * 3600;
            }
        }

        private void CalculatePace()
        {
            if (Time == 0 || Math.Abs(Distance) <= 0.001)
            {
                AveragePace = null;
            }
            else
            {
                AveragePace = (int)(Time / Distance);
            }
        }

        private void CalculateLongestDistance(IEnumerable<Stat> stats)
        {
            var longestDistanceStat = stats.OrderByDescending(stat => stat.Distance).First();
            LongestDistance = new ActivityPointer(longestDistanceStat.Id, longestDistanceStat.Time, longestDistanceStat.Distance);
        }

        private void CalculateLongestTime(IEnumerable<Stat> stats)
        {
            var longestTimeStat = stats.OrderByDescending(stat => stat.Time).First();
            LongestTime = new ActivityPointer(longestTimeStat.Id, longestTimeStat.Time, longestTimeStat.Distance);
        }
    }
}

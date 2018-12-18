using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Efforteo.Services.Stats.Domain.DTO
{
    public class CategoryTotalStatsDto
    {
        public string Category { get; set; }
        public int Amount { get; set; }
        public long Time { get; set; }
        public float Distance { get; set; }
        public float? AverageSpeed { get; set; }
        public int? AveragePace { get; set; }
        public ActivityPointer LongestDistance { get; set; }
        public ActivityPointer LongestTime { get; set; }
    }
}

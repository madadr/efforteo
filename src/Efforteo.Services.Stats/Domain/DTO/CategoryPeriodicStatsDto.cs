using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Efforteo.Services.Stats.Domain.DTO
{
    public class CategoryPeriodicStatsDto
    {
        public string Category { get; set; }
        public DateTime PeriodStart { get; set; }
        public int Days { get; set; }
        public float[] Distance { get; set; }
        public long[] Time { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Efforteo.Services.Stats.Domain.DTO
{
    public class CategoryDetailedStatsDto
    {
        public string Category { get; set; }
        public DetailedStatDto[] Stats { get; set; }
    }
}

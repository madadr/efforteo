using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Efforteo.Services.Stats.Domain.DTO
{
    public class ActivityPointer
    {
        public Guid Id { get; set; }
        public long Time { get; set; }
        public float Distance { get; set; }

        public ActivityPointer(Guid id, long time, float distance)
        {
            Id = id;
            Time = time;
            Distance = distance;
        }
    }
}

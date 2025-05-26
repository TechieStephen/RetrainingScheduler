using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetrainingSchedular.Models
{
    public class Track
    {
        public List<Talk>? MorningTalks { get; set; }
        public List<Talk>? AfternoonTalks { get; set; }
        public TimeSpan SharingTimeStart { get; set; }
    }
}

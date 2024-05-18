using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.OpenAq.Sensors
{
    public class Coverage
    {
        public int expectedCount { get; set; }
        public string expectedInterval { get; set; }
        public int observedCount { get; set; }
        public string observedInterval { get; set; }
        public double percentComplete { get; set; }
        public double percentCoverage { get; set; }
        public object datetimeFrom { get; set; }
        public object datetimeTo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.OpenAq.Sensors
{
    public class Summary
    {
        public double min { get; set; }
        public object q02 { get; set; }
        public object q25 { get; set; }
        public object median { get; set; }
        public object q75 { get; set; }
        public object q98 { get; set; }
        public double max { get; set; }
        public double avg { get; set; }
        public object sd { get; set; }
    }
}

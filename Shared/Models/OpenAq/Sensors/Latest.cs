using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.OpenAq.Sensors
{
    public class Latest
    {
        public Datetime datetime { get; set; }
        public double value { get; set; }
        public Coordinates coordinates { get; set; }
    }
}

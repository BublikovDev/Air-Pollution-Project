using Shared.Models.OpenAq.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.OpenAq.ViewData
{
    public class GetViewDataResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public string locality { get; set; }
        public string timezone { get; set; }
        public Country country { get; set; }
        public Owner owner { get; set; }
        public Provider provider { get; set; }
        public bool isMobile { get; set; }
        public bool isMonitor { get; set; }
        public List<Instrument> instruments { get; set; }
        public List<Result> sensors { get; set; }
        public Coordinates coordinates { get; set; }
        public List<License> licenses { get; set; }
        public List<double> bounds { get; set; }
        public object distance { get; set; }
        public DatetimeFirst datetimeFirst { get; set; }
        public DatetimeLast datetimeLast { get; set; }
    }
}

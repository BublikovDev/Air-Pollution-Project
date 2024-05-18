using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.OpenAq.Sensors
{
    public class Result
    {
        public int id { get; set; }
        public string name { get; set; }
        public Parameter parameter { get; set; }
        public DatetimeFirst datetimeFirst { get; set; }
        public DatetimeLast datetimeLast { get; set; }
        public Coverage coverage { get; set; }
        public Latest latest { get; set; }
        public Summary summary { get; set; }
    }
}

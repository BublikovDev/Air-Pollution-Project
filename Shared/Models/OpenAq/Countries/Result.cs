using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.OpenAq.Countries
{
    public class Result
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public DateTime datetimeFirst { get; set; }
        public DateTime datetimeLast { get; set; }
        public List<Parameter> parameters { get; set; }
    }
}

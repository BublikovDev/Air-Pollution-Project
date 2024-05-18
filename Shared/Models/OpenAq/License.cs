using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.OpenAq
{
    public class License
    {
        public int id { get; set; }
        public string url { get; set; }
        public string dateFrom { get; set; }
        public object dateTo { get; set; }
        public string description { get; set; }
    }
}

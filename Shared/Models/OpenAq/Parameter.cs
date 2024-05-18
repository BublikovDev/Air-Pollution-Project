using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.OpenAq
{
    public class Parameter
    {
        public int id { get; set; }
        public string name { get; set; }
        public string units { get; set; }
        public object displayName { get; set; }
    }
}

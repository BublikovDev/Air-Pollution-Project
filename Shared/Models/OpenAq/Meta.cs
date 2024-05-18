using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.OpenAq
{
    public class Meta
    {
        public string name { get; set; }
        public string website { get; set; }
        public int page { get; set; }
        public int limit { get; set; }
        public int found { get; set; }
    }
}

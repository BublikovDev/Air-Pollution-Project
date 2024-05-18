using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.OpenAq.Locations
{
    public class GetLocationsResponse
    {
        public Meta meta { get; set; }
        public List<Result> results { get; set; }
    }
}

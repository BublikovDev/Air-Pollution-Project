using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Models.OpenAq.Locations;

namespace Shared.Models.OpenAq.Sensors
{
    public class GetSensorsByLocationIdResponse
    {
        public Meta meta { get; set; }
        public List<Result> results { get; set; }
    }
}

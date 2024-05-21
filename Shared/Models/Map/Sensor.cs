using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Map
{
    public class Sensor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LatestTimeUtc { get; set; }
        public DateTime LatestTimeLocal { get; set; }
        public double Value { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public double AvgValue { get; set; }
        public bool IsDeleted { get; set; }=false;

        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}

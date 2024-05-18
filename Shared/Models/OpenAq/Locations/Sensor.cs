﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.OpenAq.Locations
{
    public class Sensor
    {
        public int id { get; set; }
        public string name { get; set; }
        public Parameter parameter { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Traffic_Simulation_Server
{
    public class Crossroads
    {
        public Point Point { get; set; }
        [JsonIgnore]
        public List<Car> Cars { get; set; }
    }
}

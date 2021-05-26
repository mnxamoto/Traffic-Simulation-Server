using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic_Simulation_Server
{
    public class RoadVertical : Road
    {
        public CrossroadsGrid CrossroadsUp { get; set; }
        public CrossroadsGrid CrossroadsDown { get; set; }

        public RoadVertical(CrossroadsGrid crossroadsUp, CrossroadsGrid crossroadsDown, NumberStrips numberStrips)
            : base()
        {
            CrossroadsUp = crossroadsUp;
            CrossroadsUp.RoadDown = this;
            CrossroadsDown = crossroadsDown;
            CrossroadsDown.RoadUp = this;
            NumberStrips = numberStrips;
        }
    }
}

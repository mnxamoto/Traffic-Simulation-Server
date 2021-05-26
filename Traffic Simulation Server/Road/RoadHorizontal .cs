using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic_Simulation_Server
{
    public class RoadHorizontal : Road
    {
        public CrossroadsGrid CrossroadsLeft { get; set; }
        public CrossroadsGrid CrossroadsRight { get; set; }

        public RoadHorizontal(CrossroadsGrid crossroadsLeft, CrossroadsGrid crossroadsRight, NumberStrips numberStrips)
            : base()
        {
            CrossroadsLeft = crossroadsLeft;
            CrossroadsLeft.RoadRight = this;
            CrossroadsRight = crossroadsRight;
            CrossroadsRight.RoadLeft = this;
            NumberStrips = numberStrips;
        }
    }
}

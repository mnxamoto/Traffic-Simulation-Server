using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Traffic_Simulation_Server
{
    public class CrossroadsCircle : Crossroads
    {
        public CrossroadsCircle(Point point)
        {
            Point = point;
            Cars = new List<Car>();
        }
    }
}

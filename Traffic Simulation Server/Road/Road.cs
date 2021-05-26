using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic_Simulation_Server
{
    public abstract class Road
    {
        public List<Car> Cars { get; set; }
        public NumberStrips NumberStrips { get; set; }

        public Road()
        {
            Cars = new List<Car>();
        }
    }

    public enum NumberStrips
    {
        One = 1,
        Two = 2
    }
}

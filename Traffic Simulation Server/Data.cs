using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic_Simulation_Server
{
    public class Data
    {
        private static Data instance;

        private Data()
        { }

        public static Data GetInstance()
        {
            if (instance == null)
                instance = new Data();
            return instance;
        }

        public Crossroads[,] CrossroadsArray { get; set; }
        public List<Car> Cars { get; set; }
    }
}

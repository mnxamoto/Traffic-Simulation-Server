using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic_Simulation_Server
{
    public class Packet
    {
        public Command Command;
        public string data;
    }

    public class StartInfo
    {
        public int countRow;
        public int countColumm;
        public int countCar;
        public int minSpeed;
        public int maxSpeed;
        public bool useTrafficLight;
    }

    public enum Command
    {
        StartGrid,
        StartCircle,
        GetCrossroadses,
        SendCrossroadses,
        GetCars,
        SendCars,
        Stop
    }
}

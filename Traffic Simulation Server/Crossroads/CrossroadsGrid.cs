using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Newtonsoft.Json;

namespace Traffic_Simulation_Server
{
    public class CrossroadsGrid : Crossroads
    {
        public Phase Phase { get; set; }
        [JsonIgnore]
        public Road RoadUp { get; set; }
        [JsonIgnore]
        public Road RoadDown { get; set; }
        [JsonIgnore]
        public Road RoadLeft { get; set; }
        [JsonIgnore]
        public Road RoadRight { get; set; }

        public CrossroadsGrid(Point point, Phase phase)
        {
            Point = point;
            Phase = phase;
            Cars = new List<Car>();
        }

        public void ChangePhase()
        {
            switch (Phase)
            {
                case Phase.First:

                    Phase = Phase.Second;
                    break;
                case Phase.Second:

                    Phase = Phase.First;
                    break;
                case Phase.Green:

                    break;
                default:
                    break;
            }
        }
    }

    public enum Phase
    {
        First = 0,
        Second = 1,
        Green = 2
    }
}

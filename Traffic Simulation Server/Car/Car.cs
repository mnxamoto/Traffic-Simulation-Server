using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Traffic_Simulation_Server
{
    public abstract class Car
    {
        public Point Current { get; set; }
        [JsonIgnore]
        public Crossroads Crossroads { get; set; }
        [JsonIgnore]
        public int iGrid { get; set; }
        [JsonIgnore]
        public int kGrid { get; set; }
        [JsonIgnore]
        public readonly int DelayMax = 100;
        [JsonIgnore]
        public int DelayСounter = 0;
        [JsonIgnore]
        public int DelayStep { get; set; }
        [JsonIgnore]
        public int Step = 5;
        public abstract void MakeStep();
        public abstract bool CheckCollision();
        public abstract void ChoiceDirection();
    }
}

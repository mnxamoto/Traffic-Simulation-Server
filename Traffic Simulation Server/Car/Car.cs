using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Traffic_Simulation_Server
{
    public abstract class Car
    {
        public Crossroads Crossroads { get; set; }
        public int iGrid { get; set; }
        public int kGrid { get; set; }

        public readonly int DelayMax = 100;
        public int DelayСounter = 0;
        public int DelayStep { get; set; }
        public Point Current { get; set; }
        public int Step = 5;
        public abstract void MakeStep();
        public abstract bool CheckCollision();
        public abstract void ChoiceDirection();
    }
}

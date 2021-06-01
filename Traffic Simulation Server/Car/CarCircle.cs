using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Traffic_Simulation_Server
{
    public class CarCircle : Car
    {
        public int Angle { get; set; }
        [JsonIgnore]
        public DirectionMotionCicle DirectionMotionCicle { get; set; }
        public Ring Ring { get; set; }

        private Random random;

        public CarCircle(int i, int k, int delayStep, int step, int angle)
        {
            iGrid = i;
            kGrid = k;
            Crossroads = Data.GetInstance().CrossroadsArray[i, k];
            Crossroads.Cars.Add(this);
            Step = step;
            Angle = angle;
            DelayStep = delayStep;
            random = new Random();
            DirectionMotionCicle = (DirectionMotionCicle)Enum.GetValues(typeof(DirectionMotionCicle)).GetValue(random.Next(0, 4));
            Ring = Ring.FIrst;
        }

        public override void MakeStep()
        {
            DelayСounter += DelayStep;

            if (DelayСounter > DelayMax)
            {
                if (CheckCollision())
                {
                    DelayСounter = 0;
                    return;
                }

                Angle += Step;

                if ((Angle < (int)DirectionMotionCicle) &&
                    (Angle >= (int)DirectionMotionCicle - 80) &&
                    (Ring == Ring.FIrst))
                {
                    Ring = Ring.Second;
                }
                else

                if (Angle == (int)DirectionMotionCicle)
                {
                    switch (DirectionMotionCicle)
                    {
                        case DirectionMotionCicle.Up:
                            kGrid--;
                            if (kGrid < 0)
                            {
                                kGrid = Data.GetInstance().CrossroadsArray.GetLength(1) - 1;
                            }
                            Angle = 275;
                            break;
                        case DirectionMotionCicle.Right:
                            iGrid++;
                            iGrid %= Data.GetInstance().CrossroadsArray.GetLength(0);
                            Angle = 185;
                            break;
                        case DirectionMotionCicle.Down:
                            kGrid++;
                            kGrid %= Data.GetInstance().CrossroadsArray.GetLength(1);
                            Angle = 95;
                            break;
                        case DirectionMotionCicle.Left:
                            iGrid--;
                            if (iGrid < 0)
                            {
                                iGrid = Data.GetInstance().CrossroadsArray.GetLength(1) - 1;
                            }
                            Angle = 5;
                            break;
                        default:
                            break;
                    }

                    Crossroads.Cars.Remove(this);
                    Crossroads = Data.GetInstance().CrossroadsArray[iGrid, kGrid];
                    Crossroads.Cars.Add(this);
                    ChoiceDirection();
                }

                Angle %= 360;
                double angleRadian = (360 - Angle) * Math.PI / 180;

                Point offset = new Point();
                offset.X = Crossroads.Point.X + (int)((int)Ring * Math.Cos(angleRadian));
                offset.Y = Crossroads.Point.Y + (int)((int)Ring * Math.Sin(angleRadian));

                Current = offset;

                DelayСounter = 0;
            }
        }

        public override bool CheckCollision()
        {
            bool result = false;
            int lenghtSection = 20;
            List<Car> cars = Crossroads.Cars;

            foreach (CarCircle car in cars)
            {
                if (car == this)
                {
                    continue;
                }

                if ((car.Angle > Angle) && (car.Angle < (Angle + lenghtSection)))
                {
                    result = true;
                }
            }

            return result;
        }

        public override void ChoiceDirection()
        {
            DirectionMotionCicle = (DirectionMotionCicle)Enum.GetValues(typeof(DirectionMotionCicle)).GetValue(random.Next(0, 4));
            Ring = Ring.FIrst;
        }
    }

    public enum Ring
    {
        FIrst = 30,
        Second = 40
    }

    public enum DirectionMotionCicle
    {
        Up = 85,
        Right = 355,
        Down = 265,
        Left = 175
    }
}

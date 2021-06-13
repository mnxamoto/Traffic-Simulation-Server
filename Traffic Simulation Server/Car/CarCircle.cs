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
        [JsonIgnore]
        public bool IsChangeCrossRoads { get; set; }
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
            ChoiceDirection();
            Ring = Ring.FIrst;
            IsChangeCrossRoads = false;
        }

        public override void MakeStep()
        {
            DelayСounter += DelayStep;
            Point offset;

            if (DelayСounter > DelayMax)
            {
                if (IsChangeCrossRoads)
                {
                    offset = Current;
                    Point endCrossroads = Crossroads.Point;
                    int distance = 25;

                    switch (DirectionMotionCicle)
                    {
                        case DirectionMotionCicle.Up:
                            offset.Y -= Step;
                            endCrossroads.Y += distance;
                            endCrossroads.X += 4;
                            Angle = 0;
                            break;
                        case DirectionMotionCicle.Right:
                            offset.X += Step;
                            endCrossroads.X -= distance;
                            endCrossroads.Y += 4;
                            Angle = 270;
                            break;
                        case DirectionMotionCicle.Down:
                            offset.Y += Step;
                            endCrossroads.Y -= distance;
                            endCrossroads.X -= 4;
                            Angle = 180;
                            break;
                        case DirectionMotionCicle.Left:
                            offset.X -= Step;
                            endCrossroads.X += distance;
                            endCrossroads.Y -= 4;
                            Angle = 90;
                            break;
                        default:
                            break;
                    }

                    Current = offset;

                    if (Current == endCrossroads)
                    {
                        IsChangeCrossRoads = false;

                        switch (DirectionMotionCicle)
                        {
                            case DirectionMotionCicle.Up:
                                Angle = 275;
                                break;
                            case DirectionMotionCicle.Right:
                                Angle = 185;
                                break;
                            case DirectionMotionCicle.Down:
                                Angle = 95;
                                break;
                            case DirectionMotionCicle.Left:
                                Angle = 5;
                                break;
                            default:
                                break;
                        }

                        ChoiceDirection();
                    }

                    return;
                }

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
                            break;
                        case DirectionMotionCicle.Right:
                            iGrid++;
                            break;
                        case DirectionMotionCicle.Down:
                            kGrid++;
                            break;
                        case DirectionMotionCicle.Left:
                            iGrid--;
                            break;
                        default:
                            break;
                    }

                    Crossroads.Cars.Remove(this);
                    Crossroads = Data.GetInstance().CrossroadsArray[iGrid, kGrid];
                    Crossroads.Cars.Add(this);
                    //ChoiceDirection();

                    IsChangeCrossRoads = true;
                    return;
                }

                Angle %= 360;
                double angleRadian = (360 - Angle) * Math.PI / 180;

                offset = new Point();
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
            Ring = Ring.FIrst;

            do
            {
                DirectionMotionCicle = (DirectionMotionCicle)Enum.GetValues(typeof(DirectionMotionCicle)).GetValue(random.Next(0, 4));

                switch (DirectionMotionCicle)
                {
                    case DirectionMotionCicle.Up:
                        if ((kGrid - 1) >= 0)
                        {
                            return;
                        }
                        break;
                    case DirectionMotionCicle.Right:
                        if ((iGrid + 1) < Data.GetInstance().CrossroadsArray.GetLength(0))
                        {
                            return;
                        }
                        break;
                    case DirectionMotionCicle.Down:
                        if ((kGrid + 1) < Data.GetInstance().CrossroadsArray.GetLength(1))
                        {
                            return;
                        }
                        break;
                    case DirectionMotionCicle.Left:
                        if ((iGrid - 1) >= 0)
                        {
                            return;
                        }
                        break;
                    default:
                        break;
                }
            } while (true);
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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Traffic_Simulation_Server
{
    public class CarGrid : Car
    {
        [JsonIgnore]
        public new CrossroadsGrid Crossroads { get; set; }
        public DirectionMotion DirectionMotion { get; set; }
        public DirectionMotion DirectionMotionNext { get; set; }
        [JsonIgnore]
        public Road Road { get; set; }
        [JsonIgnore]
        private Random random;

        public CarGrid(CrossroadsGrid crossroads, int delayStep, int step)
        {
            random = new Random();
            Current = crossroads.Point;
            Road = crossroads.RoadRight;
            Road.Cars.Add(this);
            Crossroads = ((RoadHorizontal)Road).CrossroadsRight;
            //Crossroads = Coordinates.GetInstance().CrossroadsArray[iGrid, kGrid];
            //Crossroads.Cars.Add(this);
            Step = step;
            DelayStep = delayStep;
            DirectionMotion = DirectionMotion.Right;
            SelectDirection();
        }

        public override bool CheckCollision()
        {
            bool result = false;
            int lenghtSection = 15;
            List<Car> cars = Road.Cars;

            Point stratSection = Current;
            Point endSection = stratSection;

            switch (DirectionMotion)
            {
                case DirectionMotion.Up:
                    endSection.Y -= lenghtSection;
                    break;
                case DirectionMotion.Right:
                    endSection.X += lenghtSection;
                    break;
                case DirectionMotion.Down:
                    endSection.Y += lenghtSection;
                    break;
                case DirectionMotion.Left:
                    endSection.X -= lenghtSection;
                    break;
                default:
                    break;
            }

            int minX = Math.Min(stratSection.X, endSection.X);
            int maxX = Math.Max(stratSection.X, endSection.X);
            int minY = Math.Min(stratSection.Y, endSection.Y);
            int maxY = Math.Max(stratSection.Y, endSection.Y);

            foreach (CarGrid car in cars)
            {
                if ((car == this) ||
                    (car.Current == Current) ||
                    (car.DirectionMotion != DirectionMotion))
                {
                    continue;
                }

                int x = car.Current.X;
                int y = car.Current.Y;

                if ((x >= minX) &&
                    (x <= maxX) &&
                    (y >= minY) &&
                    (y <= maxY))
                {
                    result = true;
                }
            }

            return result;
        }

        public override void MakeStep()
        {
            DelayСounter += DelayStep;

            if (DelayСounter > DelayMax)
            {
                if (CheckCollision() || isRedLight())
                {
                    DelayСounter = 0;
                    return;
                }

                if (Current == Crossroads.Point)
                {
                    ChoiceDirection();
                    DelayСounter = 0;
                    return;
                }

                Point offset = Current;

                switch (DirectionMotion)
                {
                    case DirectionMotion.Up:
                        offset.Y -= Step;
                        break;
                    case DirectionMotion.Right:
                        offset.X += Step;
                        break;
                    case DirectionMotion.Down:
                        offset.Y += Step;
                        break;
                    case DirectionMotion.Left:
                        offset.X -= Step;
                        break;
                    default:
                        break;
                }

                Current = offset;

                DelayСounter = 0;
            }
        }

        public override void ChoiceDirection()
        {
            Road.Cars.Remove(this);

            DirectionMotion = DirectionMotionNext;

            switch (DirectionMotion)
            {
                case DirectionMotion.Up:
                    Road = Crossroads.RoadUp;
                    Crossroads = ((RoadVertical)Road).CrossroadsUp;
                    break;
                case DirectionMotion.Right:
                    Road = Crossroads.RoadRight;
                    Crossroads = ((RoadHorizontal)Road).CrossroadsRight;
                    break;
                case DirectionMotion.Down:
                    Road = Crossroads.RoadDown;
                    Crossroads = ((RoadVertical)Road).CrossroadsDown;
                    break;
                case DirectionMotion.Left:
                    Road = Crossroads.RoadLeft;
                    Crossroads = ((RoadHorizontal)Road).CrossroadsLeft;
                    break;
                default:
                    break;
            }

            Road.Cars.Add(this);
            SelectDirection();
        }

        private void SelectDirection()
        {
            Road newRoad = null;

            do
            {
                DirectionMotionNext = (DirectionMotion)Enum.GetValues(typeof(DirectionMotion)).GetValue(random.Next(0, 4));

                switch (DirectionMotionNext)
                {
                    case DirectionMotion.Up:
                        newRoad = Crossroads.RoadUp;
                        break;
                    case DirectionMotion.Right:
                        newRoad = Crossroads.RoadRight;
                        break;
                    case DirectionMotion.Down:
                        newRoad = Crossroads.RoadDown;
                        break;
                    case DirectionMotion.Left:
                        newRoad = Crossroads.RoadLeft;
                        break;
                    default:
                        break;
                }

            } while (newRoad == null);
        }

        private bool isRedLight()
        {
            if ((DrawHelper.CalculateDistance(Current, Crossroads.Point) < 25) && (DrawHelper.CalculateDistance(Current, Crossroads.Point) > 20))
            {
                if (((Crossroads.Phase == Phase.First) && ((DirectionMotion == DirectionMotion.Down) || (DirectionMotion == DirectionMotion.Up))) ||
                    ((Crossroads.Phase == Phase.Second) && ((DirectionMotion == DirectionMotion.Left) || (DirectionMotion == DirectionMotion.Right))))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public enum DirectionMotion
    {
        Up,
        Right,
        Down,
        Left
    }
}

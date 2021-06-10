using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Traffic_Simulation_Server
{
    public static class WorkHelper
    {
        public static bool isWorkedTask;

        public static void WorkGrid(Queue<CarGrid> carsQueue, CrossroadsGrid[,] crossroadsArray)
        {
            isWorkedTask = true;

            List<Car> cars = new List<Car>();

            while (carsQueue.Count > 0)
            {
                CarGrid car = carsQueue.Dequeue();
                cars.Add(car);

                for (int i = 0; i < 20; i++)
                {
                    MotionCars(cars);
                }
            }

            Data.GetInstance().Cars = cars;
            Data.GetInstance().CrossroadsArray = crossroadsArray;

            Task.Factory.StartNew(() =>
            {
                WorkCars(Data.GetInstance().Cars);
            });

            Task.Factory.StartNew(() =>
            {
                WorkCrossroads(Data.GetInstance().CrossroadsArray);
            });
        }

        private static void WorkCars(List<Car> cars)
        {
            while (isWorkedTask)
            {
                MotionCars(cars);
                Thread.Sleep(1);
            }
        }

        private static void WorkCrossroads(Crossroads[,] crossroadsArray)
        {
            int DelayСounter = 1000;

            while (isWorkedTask)
            {
                DelayСounter++;

                if (DelayСounter > 100)
                {
                    foreach (CrossroadsGrid crossroads in crossroadsArray)
                    {
                        crossroads.ChangePhase();
                    }

                    DelayСounter = 0;
                }

                Thread.Sleep(10);
            }
        }

        private static void MotionCars(List<Car> cars)
        {
            Parallel.ForEach(cars, car => 
            { 
                car.MakeStep(); 
            });
        }

        public static void WorkCircles(List<Car> cars)
        {
            isWorkedTask = true;

            while (isWorkedTask)
            {
                foreach (var car in cars)
                {
                    car.MakeStep();
                }

                Thread.Sleep(1);
            }
        }
    }
}

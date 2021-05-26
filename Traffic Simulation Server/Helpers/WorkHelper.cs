﻿using System;
using System.Collections.Generic;
using System.Text;

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
                Data.GetInstance().Cars.Add(car);

                for (int i = 0; i < 20; i++)
                {
                    MotionCars(cars);
                }
            }

            int DelayСounter = 1000;
            Data.GetInstance().Cars = cars;

            while (isWorkedTask)
            {
                MotionCars(Data.GetInstance().Cars);

                DelayСounter++;

                if (DelayСounter > 100)
                {
                    foreach (var crossroads in crossroadsArray)
                    {
                        crossroads.ChangePhase();
                    }

                    DelayСounter = 0;
                }
            }
        }

        private static void MotionCars(List<Car> cars)
        {
            foreach (var car in cars)
            {
                car.MakeStep();
            }
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
            }
        }
    }
}
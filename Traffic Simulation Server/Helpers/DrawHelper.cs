using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Traffic_Simulation_Server
{
    public static class DrawHelper
    {
        public static CrossroadsGrid[,] DrawGrid(int countRow, int countColumn, bool useTrafficLight)
        {
            Random random = new Random(3);

            //Смещение относительно начала координат
            int xOffset = 50;
            int yOffset = 50;

            CrossroadsGrid[,] crossroadsArray = new CrossroadsGrid[countColumn, countRow];  //Создаём массив перекрёстков

            int minPhase;
            Phase phase;

            if (useTrafficLight)
            {
                minPhase = 1;
            }
            else
            {
                minPhase = 2;
            }

            for (int i = 0; i < countColumn; i++)
            {
                for (int k = 0; k < countRow; k++)
                {
                    Point point = new Point(xOffset + i * 100, yOffset + k * 100);
                    phase = (Phase)Enum.GetValues(typeof(Phase)).GetValue(random.Next(minPhase, 3));
                    CrossroadsGrid crossroads = new CrossroadsGrid(point, phase);

                    crossroadsArray[i, k] = crossroads;
                }
            }

            Data.GetInstance().crossroadsArray = crossroadsArray;

            Point start;
            Point end;
            Size size;
            Rectangle rectangle;

            NumberStrips numberStrips;

            for (int i = 0; i < countColumn; i++)
            {
                for (int k = 0; k < countRow - 1; k++)
                {
                    numberStrips = (NumberStrips)Enum.GetValues(typeof(NumberStrips)).GetValue(random.Next(1, 2));

                    RoadVertical road = new RoadVertical(
                        crossroadsArray[i, k],
                        crossroadsArray[i, k + 1],
                        numberStrips);
                }
            }

            for (int k = 0; k < countRow; k++)
            {
                for (int i = 0; i < countColumn - 1; i++)
                {
                    numberStrips = (NumberStrips)Enum.GetValues(typeof(NumberStrips)).GetValue(random.Next(1, 2));

                    RoadHorizontal road = new RoadHorizontal(
                        crossroadsArray[i, k],
                        crossroadsArray[i + 1, k],
                        numberStrips);
                }
            }

            return crossroadsArray;
        }

        public static void DrawCircles(int countRow, int countColumn)
        {
            //Смещение относительно начала координат
            int xOffset = 50;
            int yOffset = 50;

            int[] radius = new int[3] { 45, 35, 25 };

            CrossroadsCircle[,] crossroadsCircleArray = new CrossroadsCircle[countColumn, countRow];  //Создаём массив кругов

            for (int i = 0; i < countColumn; i++)
            {
                for (int k = 0; k < countRow; k++)
                {
                    Point point = new Point(xOffset + i * 100 + radius[0], yOffset + k * 100 + radius[0]);
                    CrossroadsCircle crossroadsCircle = new CrossroadsCircle(point);
                    crossroadsCircleArray[i, k] = crossroadsCircle;
                }
            }

            Data.GetInstance().CrossroadsCircleArray = crossroadsCircleArray;
        }

        public static int CalculateDistance(Point p1, Point p2)
        {
            return (int)Math.Abs(Math.Pow(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2), 0.5));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConwaysGameOfLife
{
    class GameOfLife
    {
        static void Main(string[] args)
        {
            //grid parameters
            int xAxis = 50;
            int yAxis = 30;
            int maxGenerations = 20;
            int i = 1;
            int refreshDelayMs = 1500;

            //cursor management
            int x = 0;
            int y = 0;

            var rand = new Random();

            //to store cell information
            List<Point> livingCells = new List<Point>();
            List<Point> deadCells = new List<Point>();
            List<Point> neighborCells = new List<Point>();

            //generate starter grid with random living cells
            while (y <= yAxis)
            {
                if (x > xAxis)
                {
                    y++;
                    x = 0;
                    Console.Write("\r\n");
                }

                if (rand.Next(10) == 5)
                {
                    livingCells.Add(new Point(x, y));
                    Console.Write("X");
                }

                else
                {
                    deadCells.Add(new Point(x, y));
                    Console.Write("=");
                }

                x++;
            }

            Thread.Sleep(refreshDelayMs);
            Console.Clear();

            while (i <= maxGenerations)
            {
                var nextGen = GetNextGen(xAxis, yAxis, livingCells, deadCells);
                livingCells = nextGen.First();
                deadCells = nextGen.Last();
                renderCurrentGen(xAxis, yAxis, livingCells);
                i++;
                Thread.Sleep(refreshDelayMs);
                Console.Clear();
            }

            Console.ReadKey();
        }

        public static void renderCurrentGen(int xAxis, int yAxis, List<Point> livingCells)
        {

            int x = 0;
            int y = 0;

            Point currentPoint = new Point(x, y);

            while (currentPoint.Y <= yAxis)
            {
                if (currentPoint.X > xAxis)
                {
                    currentPoint.Y++;
                    currentPoint.X = x;
                    Console.Write("\r\n");
                }

                if (livingCells.Contains(currentPoint))
                    Console.Write("X");

                else
                    Console.Write("=");

                currentPoint.X++;
            }
        }

        public static IEnumerable<List<Point>> GetNextGen(int xAxis, int yAxis, List<Point> livingCells, List<Point> deadCells)
        {
            //compare neighbors to find next gen
            int numNeighbors = 0;
            int x = 0;
            int y = 0;
            Point currentPoint = new Point(x, y);
            Point neighborPoint;
            while (currentPoint.Y <= yAxis)
            {
                if (currentPoint.X > xAxis)
                {
                    currentPoint.Y++;
                    currentPoint.X = x;
                }

                neighborPoint = currentPoint;
                //check cell to the left of currentPoint
                neighborPoint.X--;
                if (livingCells.Contains(neighborPoint))
                    numNeighbors++;

                //check cell to the right
                neighborPoint = currentPoint;
                neighborPoint.X++;
                if (livingCells.Contains(neighborPoint))
                    numNeighbors++;

                //check cell above
                neighborPoint = currentPoint;
                neighborPoint.Y++;
                if (livingCells.Contains(neighborPoint))
                    numNeighbors++;

                //check cell below
                neighborPoint = currentPoint;
                neighborPoint.Y--;
                if (livingCells.Contains(neighborPoint))
                    numNeighbors++;

                //check cell diagonal bottom left
                neighborPoint = currentPoint;
                neighborPoint.X--;
                neighborPoint.Y--;
                if (livingCells.Contains(neighborPoint))
                    numNeighbors++;

                //check cell diagonal bottom right
                neighborPoint = currentPoint;
                neighborPoint.X++;
                neighborPoint.Y--;
                if (livingCells.Contains(neighborPoint))
                    numNeighbors++;

                //check cell diagonal top left
                neighborPoint = currentPoint;
                neighborPoint.X--;
                neighborPoint.Y++;
                if (livingCells.Contains(neighborPoint))
                    numNeighbors++;

                //check cell diagonal top right
                neighborPoint = currentPoint;
                neighborPoint.X++;
                neighborPoint.Y++;
                if (livingCells.Contains(neighborPoint))
                    numNeighbors++;

                if (numNeighbors != 2 && numNeighbors != 3 && livingCells.Contains(currentPoint))
                {
                    livingCells.Remove(currentPoint);
                    deadCells.Add(currentPoint);
                }

                else if (numNeighbors == 3 && deadCells.Contains(currentPoint))
                {
                    livingCells.Add(currentPoint);
                    deadCells.Remove(currentPoint);
                }

                numNeighbors = 0;


                currentPoint.X++;
            }
            return new List<List<Point>> { livingCells, deadCells };
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day6
{
    class Program
    {
        class Point
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Point(string line)
            {
                var parts = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                X = Convert.ToInt32(parts[0]);
                Y = Convert.ToInt32(parts[1]);
            }
        }

        static List<Point> LoadData()
        {
            return File.ReadLines("input.txt").Select(q => new Point(q)).ToList();
        }

        static int GetNearestPoint(int x, int y, List<Point> data)
        {
            var calc = data.Select((q, i) => new { index = i, distance = Math.Abs(x - q.X) + Math.Abs(y - q.Y) }).ToList().OrderBy(q => q.distance).ToList();

            if (calc[0].distance == calc[1].distance)
                return -1;
            else
                return calc[0].index;
        }

        static int GetSumDistance(int x, int y, List<Point> data)
        {
            return data.Sum(q => Math.Abs(x - q.X) + Math.Abs(y - q.Y));
        }

        static int[,] BuildMap(List<Point> data, Func<int, int, List<Point>, int> function)
        {
            // find offsets
            var xMin = data.Min(q => q.X) - 1;
            var yMin = data.Min(q => q.Y) - 1;
            var xMax = data.Max(q => q.X) + 1;
            var yMax = data.Max(q => q.Y) + 1;
            var width = xMax - xMin + 1;
            var height = yMax - yMin + 1;

            // init array
            var map = new int[width, height];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    map[x, y] = function(x + xMin, y + yMin, data);

            return map;
        }

        static int CalculatePart1(List<Point> data)
        {
            var map = BuildMap(data, GetNearestPoint);

            var width = map.GetLength(0);
            var height = map.GetLength(1);
            var count = new Dictionary<int, int>();
            var infinite = new HashSet<int>();
            // count the cells per point
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    // if the cell contains a valid point
                    int point = map[x, y];
                    if (point > -1)
                        // if the coordinates are at the borders, then add the point to the infinte list 
                        if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                            infinite.Add(point);
                        else
                        {
                            // Otherwise increment the count for the point
                            if (count.TryGetValue(point, out int buffer))
                                count[point] = buffer + 1;
                            else
                                count[point] = 1;
                        }
                }

            // sort the counts in descending order and then get the first point, that is not in the infinite hashtable
            return count.Where(q => !infinite.Contains(q.Key)).OrderByDescending(q => q.Value).FirstOrDefault().Value;
        }

        static int CalculatePart2(List<Point> data)
        {
            var map = BuildMap(data, GetSumDistance);

            var width = map.GetLength(0);
            var height = map.GetLength(1);
            var count = 0;
            // Count all the cells with a value less then 10.000
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    if (map[x, y] < 10000)
                        count++;

            return count;
        }


        static void Main(string[] args)
        {
            var data = LoadData();

            Console.WriteLine($"Part1: {CalculatePart1(data)}");

            Console.WriteLine($"Part2: {CalculatePart2(data)}");
        }
    }
}

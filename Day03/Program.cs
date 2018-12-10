using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day3
{
    class Program
    {
        class LineValues
        {
            public int ID { get; private set; }
            public int Left { get; private set; }
            public int Top { get; private set; }
            public int Width { get; private set; }
            public int Heigth { get; private set; }

            public LineValues(string line)
            {
                var split = line.Split(new String[] { "#", "@", ",", ":", "x" }, StringSplitOptions.RemoveEmptyEntries);
                ID = Convert.ToInt32(split[0].Trim());
                Left = Convert.ToInt32(split[1].Trim());
                Top = Convert.ToInt32(split[2].Trim());
                Width = Convert.ToInt32(split[3].Trim());
                Heigth = Convert.ToInt32(split[4].Trim());
            }
        }

        static List<LineValues> LoadData()
        {
            return File.ReadLines("input.txt").Select(q => new LineValues(q)).ToList();
        }

        static int CalculatePart1(List<LineValues> data)
        {
            var yaxis = new Dictionary<int, Dictionary<int, int>>();

            foreach (var line in data)
            {
                for (int y = line.Top; y < line.Top + line.Heigth; y++)
                    for (int x = line.Left; x < line.Left + line.Width; x++)
                    {
                        Dictionary<int, int> xaxis;
                        if (!yaxis.TryGetValue(y, out xaxis))
                        {
                            xaxis = new Dictionary<int, int>();
                            yaxis[y] = xaxis;
                        }
                        int count;
                        if (!xaxis.TryGetValue(x, out count))
                            xaxis[x] = 1;
                        else
                            xaxis[x] = count + 1;
                    }
            }

            // Count all the cells higher then 1
            int countOverlaps = 0;
            foreach (var xaxis in yaxis.Values)
                foreach (var x in xaxis.Values)
                    if (x > 1)
                        countOverlaps++;

            return countOverlaps;
        }

        static int CalculatePart2(List<LineValues> data)
        {
            var yaxis = new Dictionary<int, Dictionary<int, List<int>>>();
            var ids = new HashSet<int>();
            foreach (var line in data)
            {
                ids.Add(line.ID);
                for (int y = line.Top; y < line.Top + line.Heigth; y++)
                    for (int x = line.Left; x < line.Left + line.Width; x++)
                    {
                        Dictionary<int, List<int>> xaxis;
                        if (!yaxis.TryGetValue(y, out xaxis))
                        {
                            xaxis = new Dictionary<int, List<int>>();
                            yaxis[y] = xaxis;
                        }
                        List<int> idlist;
                        if (!xaxis.TryGetValue(x, out idlist))
                            xaxis[x] = new List<int> { line.ID };
                        else
                            idlist.Add(line.ID);
                    }
            }

            // Find all cells with more then one IDs and remove them from the ids list
            foreach (var xaxis in yaxis.Values)
                foreach (var idlist in xaxis.Values.Where(q => q.Count > 1))
                    foreach (var id in idlist)
                        ids.Remove(id);

            return ids.First();
        }

        static void Main(string[] args)
        {
            var data = LoadData();

            Console.WriteLine($"Part1: {CalculatePart1(data)}");

            Console.WriteLine($"Part2: {CalculatePart2(data)}");
        }
    }
}

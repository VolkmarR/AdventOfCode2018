using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day23
{
    class Program
    {
        class Bot
        {
            public int x;
            public int y;
            public int z;
            public int r;

            public Bot(string line)
            {
                var parts = line.Split(new char[] { '=', '<', '>', '=', ',' }, StringSplitOptions.RemoveEmptyEntries);
                x = Convert.ToInt32(parts[1]);
                y = Convert.ToInt32(parts[2]);
                z = Convert.ToInt32(parts[3]);
                r = Convert.ToInt32(parts[5]);
            }

            public int Distance(Bot other)
            {
                return Math.Abs(x - other.x) + Math.Abs(y - other.y) + Math.Abs(z - other.z);
            }

            public bool InRange(Bot other)
            {
                return Distance(other) <= r;
            }
        }


        static List<Bot> LoadData()
        {
            return File.ReadAllLines("input.txt").Select(q => new Bot(q)).ToList();
        }

        static int CalculatePart1(List<Bot> data)
        {
            var maxRangeBot = data.OrderByDescending(q => q.r).First();
            return data.Count(q => maxRangeBot.InRange(q));
        }

        static void Main(string[] args)
        {
            var data = LoadData();
            Console.WriteLine($"Part1 : {CalculatePart1(data)}");
        }
    }
}

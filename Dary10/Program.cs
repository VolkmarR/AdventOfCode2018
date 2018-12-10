using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dary10
{
    class Program
    {
        class Element
        {
            public int StartX { get; set; }
            public int StartY { get; set; }
            public int VelocityX { get; set; }
            public int VelocityY { get; set; }
            public int CurrentX { get; set; }
            public int CurrentY { get; set; }

            public Element(string line)
            {
                var parts = line.Split(new char[] { '<', ',', '>' }, StringSplitOptions.RemoveEmptyEntries);
                StartX = Convert.ToInt32(parts[1].Trim());
                StartY = Convert.ToInt32(parts[2].Trim());
                VelocityX = Convert.ToInt32(parts[4].Trim());
                VelocityY = Convert.ToInt32(parts[5].Trim());

                CurrentX = StartX;
                CurrentY = StartY;
            }

            public void Move()
            {
                CurrentX += VelocityX;
                CurrentY += VelocityY;
            }
        }

        static List<Element> LoadData()
        {
            return File.ReadLines("input.txt").Select(q => new Element(q)).ToList();
        }

        static void Move(List<Element> data)
        {
            foreach (var item in data)
                item.Move();
        }

        static bool IsMessage(List<Element> data)
        {
            // For the message to appear, all the coordinates must be within a limited range. 
            // Lets assume from the example, that the height of the message ist smaller then 10 "pixels"
            var minY = data.Min(q => q.CurrentY);
            var maxY = data.Max(q => q.CurrentY);
            return Math.Abs(maxY - minY) < 10;
        }

        static void Print(List<Element> data)
        {
            var minX = data.Min(q => q.CurrentX);
            var minY = data.Min(q => q.CurrentY);
            var maxX = data.Max(q => q.CurrentX);
            var maxY = data.Max(q => q.CurrentY);

            var lines = new List<List<Char>>();
            for (int i = minY; i <= maxY; i++)
                lines.Add(new String(' ', maxX - minX + 1).ToList());

            foreach (var item in data)
                lines[item.CurrentY - minY][item.CurrentX - minX] = '#';

            for (int i = 0; i <= maxY - minY; i++)
                Console.WriteLine(String.Concat(lines[i]));
        }

        static void Main(string[] args)
        {
            var data = LoadData();
            int i = 0;
            for (; i < 1000000 && !IsMessage(data); i++)
                Move(data);
            Console.WriteLine("Answer 1:");
            Print(data);

            Console.WriteLine($"Answer 2: {i}");
        }
    }
}

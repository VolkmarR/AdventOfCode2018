using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    class Program
    {
        class Sample
        {
            public int[] Before { get; set; }
            public int[] After { get; set; }
            public int[] Command { get; set; }

            public Sample(string before, string command, string after)
            {
                before = before.Substring(9, before.Length - 10);
                after = after.Substring(9, after.Length - 10);
                Before = before.Split(',').Select(q => Convert.ToInt32(q.Trim())).ToArray();
                After = after.Split(',').Select(q => Convert.ToInt32(q.Trim())).ToArray();
                Command = command.Split(' ').Select(q => Convert.ToInt32(q.Trim())).ToArray();
            }
        }

        static List<Sample> LoadData()
        {
            var result = new List<Sample>();
            var data = File.ReadAllLines("input.txt");
            var i = 0;
            while (i < data.Length && data[i].StartsWith("Before"))
            {
                result.Add(new Sample(data[i], data[i + 1], data[i + 2]));
                i += 4;
            }

            return result;
        }

        static int CalculatePart1(List<Sample> data)
        {

            return 0;
        }


        static void Main(string[] args)
        {
            Console.WriteLine($"part 1 {CalculatePart1(LoadData())}");

        }
    }
}

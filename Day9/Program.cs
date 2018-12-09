using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day9
{
    class Program
    {
        private static List<string> LoadData()
        {
            return File.ReadLines("Input.txt").ToList();
        }

        private static string CalculatePart1(List<string> data)
        {
            return "";
        }

        private static string CalculatePart2(List<string> data)
        {
            return "";
        }

        static void Main(string[] args)
        {
            var data = LoadData();

            Console.WriteLine($"Part1: {CalculatePart1(data)}");

            Console.WriteLine($"Part2: {CalculatePart2(data)}");
        }
    }
}

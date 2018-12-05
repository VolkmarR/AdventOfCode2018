using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day5
{
    class Program
    {
        static string LoadData()
        {
            return File.ReadLines("input.txt").First();
        }

        private static List<char> React(string data)
        {
            return React(data.ToList(), (char)0);
        }

        private static List<char> React(List<char> data, char remove)
        {
            remove = Char.ToLower(remove);
            var units = new List<char>();
            foreach (var unit in data)
                if (Char.ToLower(unit) != remove)
                {
                    int lastUnit = units.Count - 1;
                    if (lastUnit > -1 &&
                        Char.ToLower(units[lastUnit]) == char.ToLower(unit) &&
                        Char.IsLower(units[lastUnit]) != Char.IsLower(unit))
                        units.RemoveAt(lastUnit);
                    else
                        units.Add(unit);
                }

            return units;
        }

        static int CalculatePart1(string data)
        {
            return React(data).Count;
        }

        static int CalculatePart2(string data)
        {
            var firstReaction = React(data);

            var availableUnitTypes = firstReaction.Select(q => Char.ToLower(q)).Distinct().ToList();
            var shortestReaction = firstReaction;
            foreach (var unitType in availableUnitTypes)
            {
                var secondReaction = React(firstReaction, unitType);
                if (secondReaction.Count < shortestReaction.Count)
                    shortestReaction = secondReaction;
            }

            return shortestReaction.Count;
        }

        static void Main(string[] args)
        {
            var data = LoadData();

            Console.WriteLine($"Part1: {CalculatePart1(data)}");

            Console.WriteLine($"Part2: {CalculatePart2(data)}");
        }
    }
}

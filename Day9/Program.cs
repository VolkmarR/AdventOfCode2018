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
        class Data
        {
            public int PlayerCount { get; set; }
            public int LastMarble { get; set; }
        }

        class Game
        {
            private List<int> data { get; set; } = new List<int> { 0 };
            private int currentPos = 0;

            public int PlayRount(int marble)
            {
                if (marble % 23 == 0)
                {
                    currentPos -= 7;
                    if (currentPos < 0)
                        currentPos += data.Count;
                    var points = data[currentPos] + marble;
                    data.RemoveAt(currentPos);
                    return points;
                }

                if (currentPos == data.Count - 2 || data.Count == 1)
                    data.Add(marble);
                else if (currentPos == data.Count - 1)
                    data.Insert(1, marble);
                else
                    data.Insert(currentPos + 2, marble);

                currentPos = data.IndexOf(marble);
                return 0;
            }

            public override string ToString()
            {
                return string.Join(" ", data);
            }
        }

        private static Data LoadData()
        {
            var parts = File.ReadLines("Input.txt").First().Split(' ');
            return new Data { PlayerCount = Convert.ToInt32(parts[0]), LastMarble = Convert.ToInt32(parts[6]) };
        }

        private static int CalculatePart1(Data data)
        {
            var game = new Game();
            var player = 1;
            var points = new Dictionary<int, int>();
            for (int i = 1; i <= data.PlayerCount; i++)
                points[i] = 0;

            for (int marble = 1; marble <= data.LastMarble; marble++)
            {
                points[player] = points[player] + game.PlayRount(marble);

                player = player < data.PlayerCount ? player + 1 : 1;
            }

            return points.Values.Max();
        }

        private static string CalculatePart2(Data data)
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

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
            private LinkedList<int> data { get; set; }
            private LinkedListNode<int> currentNode;
            private int nextModulo23 = 23;

            public int PlayRount(int marble)
            {
                if (marble == nextModulo23)
                {
                    nextModulo23 += 23;
                    var remove = currentNode;
                    for (int i = 0; i < 7; i++)
                        if (remove.Previous != null)
                            remove = remove.Previous;
                        else
                            remove = data.Last;
                    
                    var points = remove.Value + marble;
                    if (remove.Next != null)
                        currentNode = remove.Next;
                    else
                        currentNode = data.First;

                    data.Remove(remove);
                    return points;
                }

                if (currentNode.Next == null)
                    currentNode = data.First;
                else
                    currentNode = currentNode.Next;

                currentNode = data.AddAfter(currentNode, marble);
                return 0;
            }

            public override string ToString()
            {
                return string.Join(" ", data);
            }

            public Game()
            {
                data = new LinkedList<int>();
                currentNode = data.AddFirst(0);
            }

        }

        private static Data LoadData()
        {
            var parts = File.ReadLines("Input.txt").First().Split(' ');
            return new Data { PlayerCount = Convert.ToInt32(parts[0]), LastMarble = Convert.ToInt32(parts[6]) };
        }

        private static long CalculatePart1(Data data)
        {
            var game = new Game();
            var player = 1;
            var points = new Dictionary<int, long>();
            for (int i = 1; i <= data.PlayerCount; i++)
                points[i] = 0;

            for (int marble = 1; marble <= data.LastMarble; marble++)
            {
                var newPoints = game.PlayRount(marble);
                if (newPoints > 0)
                    points[player] += newPoints;

                player = player < data.PlayerCount ? player + 1 : 1;
            }

            return points.Values.Max();
        }

        private static long CalculatePart2(Data data)
        {
            data.LastMarble = data.LastMarble * 100;
            return CalculatePart1(data);
        }

        static void Main(string[] args)
        {
            var data = LoadData();

            Console.WriteLine($"Part1: {CalculatePart1(data)}");

            Console.WriteLine($"Part2: {CalculatePart2(data)}");
        }
    }
}

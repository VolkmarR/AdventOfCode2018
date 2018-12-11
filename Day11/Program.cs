using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    class Program
    {
        class Result
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Size { get; set; }
            public int Power { get; set; }
        }

        static int[,] LoadData()
        {
            var SerialNumber = Convert.ToInt32(File.ReadLines("input.txt").First());
            var result = new int[300, 300];

            for (int y = 1; y <= result.GetLength(0); y++)
                for (int x = 1; x <= result.GetLength(1); x++)
                {
                    var rackID = (x) + 10;
                    long powerLevel = rackID * (y);
                    powerLevel = powerLevel + SerialNumber;
                    powerLevel = powerLevel * rackID;
                    var digit = (int)((powerLevel % 1000) / 100);
                    SetValue(result, x, y, digit - 5);
                }

            return result;
        }

        static int GetValue(int[,] data, int x, int y)
        {
            return data[x - 1, y - 1];
        }

        static void SetValue(int[,] data, int x, int y, int value)
        {
            data[x - 1, y - 1] = value;
        }

        static string CalculatePart1(int[,] data)
        {
            var result = FindMaxPowerBox(data, 3, 3);
            return $"{result.X},{result.Y}";
        }

        static String CalculatePart2(int[,] data)
        {
            var result = FindMaxPowerBox(data, 1, 300);
            return $"{result.X},{result.Y},{result.Size}";
        }

        private static Result FindMaxPowerBox(int[,] data, int fromSize, int toSize)
        {
            var resultList = new List<Result>();
            var buffer = new int[300, 300];
            for (int size = fromSize; size <= toSize; size++)
            {
                var result = new Result { Size = size };
                resultList.Add(result);
                for (int y = 1; y < data.GetLength(0) - size; y++)
                    for (int x = 1; x < data.GetLength(1) - size; x++)
                    {
                        var power = 0;
                        if (size == fromSize)
                        {
                            for (int yo = 0; yo < size; yo++)
                                for (int xo = 0; xo < size; xo++)
                                    power += GetValue(data, x + xo, y + yo);
                        }
                        else
                        {
                            // Optimization: Reuse the values of the last round and add the numbers of the right and bottom border
                            power = GetValue(buffer, x, y);
                            for (int yo = 0; yo < size; yo++)
                                power += GetValue(data, x + size - 1, y + yo);
                            for (int xo = 0; xo < size - 1; xo++)
                                power += GetValue(data, x + xo, y + size - 1);
                        }
                        SetValue(buffer, x, y, power);

                        if (power > result.Power)
                        {
                            result.Power = power;
                            result.X = x;
                            result.Y = y;
                        }

                    }
            }
            return resultList.OrderByDescending(q => q.Power).First();
        }

        static void Main(string[] args)
        {
            var data = LoadData();

            Console.WriteLine($"Part1: {CalculatePart1(data)}");
            Console.WriteLine($"Part2: {CalculatePart2(data)}");

        }
    }
}

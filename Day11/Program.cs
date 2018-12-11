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
            var result = FindMaxPowerBox(data, 3);
            return $"{result.X},{result.Y}";
        }

        static String CalculatePart2(int[,] data)
        {
            var result = new Result { };
            for (int size = 1; size <= data.GetLength(0); size++)
            {
                var sizeResult = FindMaxPowerBox(data, size);
                if (sizeResult.Power > result.Power)
                    result = sizeResult;
            }
            return $"{result.X},{result.Y},{result.Size}";
        }


        private static Result FindMaxPowerBox(int[,] data, int size)
        {
            var result = new Result { Size = size };
            for (int y = 1; y < data.GetLength(0) - size; y++)
                for (int x = 1; x < data.GetLength(1) - size; x++)
                {
                    var power = 0;
                    for (int yo = 0; yo < size; yo++)
                        for (int xo = 0; xo < size; xo++)
                            power += GetValue(data, x + xo, y + yo);

                    if (power > result.Power)
                    {
                        result.Power = power;
                        result.X = x;
                        result.Y = y;
                    }

                }

            return result;
        }

        static void Main(string[] args)
        {
            var data = LoadData();

            Console.WriteLine($"Part1: {CalculatePart1(data)}");
            Console.WriteLine($"Part2: {CalculatePart2(data)}");

        }
    }
}

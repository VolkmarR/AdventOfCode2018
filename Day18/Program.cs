using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day18
{
    class Program
    {

        class Field
        {
            const char OpenGround = '.';
            const char Trees = '|';
            const char Lumberyard = '#';

            private char[,] Data;

            public Field(String[] lines)
            {
                Data = new char[lines[0].Length, lines.Length];
                for (var y = 0; y < lines.Length; y++)
                    for (var x = 0; x < lines[y].Length; x++)
                        Data[x, y] = lines[y][x];
            }

            private bool Equal(char[,] a1, char[,] a2)
            {
                for (var y = 0; y < Data.GetLength(1); y++)
                    for (var x = 0; x < Data.GetLength(0); x++)
                        if (a1[x, y] != a2[x, y])
                            return false;

                return true;
            }

            private IEnumerable<char> GetAdjacent(int x, int y)
            {
                var minX = Math.Max(x - 1, 0);
                var maxX = Math.Min(x + 2, Data.GetLength(0));
                var minY = Math.Max(y - 1, 0);
                var maxY = Math.Min(y + 2, Data.GetLength(1));

                for (var yy = minY; yy < maxY; yy++)
                    for (var xx = minX; xx < maxX; xx++)
                        if (xx != x || yy != y)
                            yield return Data[xx, yy];
            }

            private bool isOpenGroundToTree(int x, int y)
            {
                byte count = 0;
                foreach (var item in GetAdjacent(x, y))
                    if (item == Trees)
                    {
                        count++;
                        if (count == 3)
                            return true;
                    }
                return false;
            }

            private bool isTreeToLumberyard(int x, int y)
            {
                byte count = 0;
                foreach (var item in GetAdjacent(x, y))
                    if (item == Lumberyard)
                    {
                        count++;
                        if (count == 3)
                            return true;
                    }
                return false;
            }

            private bool isLumberyardToOpen(int x, int y)
            {
                byte trees = 0;
                byte lumberyards = 0;
                foreach (var item in GetAdjacent(x, y))
                    if (item == Trees)
                        trees++;
                    else if (item == Lumberyard)
                        lumberyards++;
                return !(lumberyards >= 1 && trees >= 1);
            }

            public void Transform(int times = 1)
            {
                var seen = new List<char[,]>();
                                
                for (var i = 0; i < times; i++)
                {
                    var newData = new Char[Data.GetLength(0), Data.GetLength(1)];
                    for (var y = 0; y < Data.GetLength(1); y++)
                        for (var x = 0; x < Data.GetLength(0); x++)
                        {
                            var item = Data[x, y];
                            if (item == OpenGround && isOpenGroundToTree(x, y))
                                newData[x, y] = Trees;
                            else if (item == Trees && isTreeToLumberyard(x, y))
                                newData[x, y] = Lumberyard;
                            else if (item == Lumberyard && isLumberyardToOpen(x, y))
                                newData[x, y] = OpenGround;
                            else
                                newData[x, y] = item;
                        }

                    // Check, if the current field configuration did already exist. 
                    for (var j = 0; j < seen.Count; j++)
                        if (Equal(seen[j], newData))
                        {
                            // in that case, jump ahead
                            var jump = i - j;
                            while (i + jump < times)
                                i += jump;
                        }

                    seen.Add(newData);
                    Data = newData;
                }
            }

            public long SumResources()
            {
                int trees = 0;
                int lumberyards = 0;

                for (var y = 0; y < Data.GetLength(1); y++)
                    for (var x = 0; x < Data.GetLength(0); x++)
                        if (Data[x, y] == Trees)
                            trees++;
                        else if (Data[x, y] == Lumberyard)
                            lumberyards++;

                return trees * lumberyards;
            }

            public void Dump()
            {
                var sb = new StringBuilder();
                for (var y = 0; y < Data.GetLength(1); y++)
                {
                    for (var x = 0; x < Data.GetLength(0); x++)
                        sb.Append(Data[x, y]);
                    sb.AppendLine();
                }

                Console.WriteLine(sb.ToString());
            }
        }

        static long CalculatePart1(string[] lines)
        {
            var data = new Field(lines);
            data.Transform(10);

            return data.SumResources();
        }

        static long CalculatePart2(string[] lines)
        {
            var data = new Field(lines);
            data.Transform(1000000000);

            return data.SumResources();
        }


        static string[] LoadData()
        {
            return File.ReadAllLines("input.txt");


        }

        static void Main(string[] args)
        {
            var data = LoadData();
            Console.WriteLine(String.Format("Part1: {0}", CalculatePart1(data)));
            Console.WriteLine(String.Format("Part2: {0}", CalculatePart2(data)));
        }
    }
}

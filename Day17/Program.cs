using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    class Program
    {
        class Reservoir
        {
            private readonly char[,] Field;
            private readonly int MaxY;
            private readonly int MinX;

            static Tuple<int, int, int, int> ParseLine(string line)
            {
                var parts = line.Split(',');
                var part1 = parts[0].Trim().Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                var part2 = parts[1].Trim().Split(new char[] { '=', '.' }, StringSplitOptions.RemoveEmptyEntries);
                var p1 = Convert.ToInt32(part1[1]);
                var p21 = Convert.ToInt32(part2[1]);
                var p22 = Convert.ToInt32(part2[2]);

                if (part1[0] == "x")
                    return new Tuple<int, int, int, int>(p1, p1, p21, p22);
                else
                    return new Tuple<int, int, int, int>(p21, p22, p1, p1);
            }

            public Reservoir(string[] lines)
            {
                var list = lines.Select(q => ParseLine(q)).ToList();
                MinX = list.Min(q => q.Item1) - 10;
                MaxY = list.Max(q => q.Item4) + 1;
                var MaxX = list.Max(q => q.Item2) + 10;

                Field = new char[MaxX - MinX + 1, MaxY + 1];
                for (var x = 0; x <= MaxX - MinX; x++)
                    for (var y = 0; y < MaxY + 1; y++)
                        Field[x, y] = ' ';

                foreach (var item in list)
                {
                    for (var x = item.Item1 - MinX; x <= item.Item2 - MinX; x++)
                        Field[x, item.Item3] = '#';
                    for (var y = item.Item3; y <= item.Item4; y++)
                        Field[item.Item1 - MinX, y] = '#';
                }
            }

            private bool isChars(int x, int y, params char[] canFlowChars)
            {
                return canFlowChars.Contains(Field[x, y]);
            }

            private int? CanFillLine(int x, int y, int dir)
            {
                var maxX = Field.GetLength(0);
                while (x > 1 && x < maxX - 1)
                {
                    if (isChars(x, y + 1, ' '))
                        return null;
                    if (!isChars(x + dir, y, ' ', '|'))
                        return x;

                    x += dir;
                }
                return null;
            }
            private bool FillLine(int x, int y)
            {
                var startX = CanFillLine(x, y, -1);
                var endX = CanFillLine(x, y, 1);
                if (startX == null || endX == null)
                    return false;

                for (var fillX = startX.Value; fillX <= endX.Value; fillX++)
                    Field[fillX, y] = '~';
                return true;
            }

            private void RunWater(int x, int y)
            {
                if (y == MaxY || Field[x, y] != ' ')
                    return;

                Field[x, y] = '|';
                RunWater(x, y + 1);

                if (isChars(x, y + 1, '#', '~'))
                {
                    RunWater(x - 1, y);
                    RunWater(x + 1, y);

                    FillLine(x, y);
                }
            }

            public void StartWater(int x, int y)
            {
                RunWater(x - MinX, y);
            }

            public int CountWater(bool countRunning)
            {
                // find start and end
                int? StartY = null;
                int? EndY = null;
                for (var y = 0; y < Field.GetLength(1); y++)
                    for (var x = 0; x < Field.GetLength(0); x++)
                        if (Field[x, y] == '#')
                        {
                            if (StartY == null)
                                StartY = y; 
                            EndY = y;
                            break;
                        }
                
                // Count Water between start and end
                var result = 0;
                for (var y = StartY.Value; y <= EndY.Value; y++)
                    for (var x = 0; x < Field.GetLength(0); x++)
                        if (Field[x, y] == '~' || (Field[x, y] == '|' && countRunning))
                            result++;
                return result;
            }
        }

        static string[] LoadData()
        {
            return File.ReadAllLines("input.txt");
        }

        static void Main(string[] args)
        {
            var data = new Reservoir(LoadData());
            data.StartWater(500, 1);
            Console.WriteLine(String.Format("Part 1: {0}", data.CountWater(true)));
            Console.WriteLine(String.Format("Part 2: {0}", data.CountWater(false)));
        }

    }
}

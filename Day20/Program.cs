using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day20
{
    class Program
    {
        class Map
        {
            public char[,] Data = new char[1000, 1000];
            public int StartX = 500;
            public int StartY = 500;

            public Map()
            {
                for (int y = 0; y < Data.GetLength(1); y++)
                    for (int x = 0; x < Data.GetLength(0); x++)
                        Data[x, y] = '#';

                Data[StartX, StartY] = 'X';
            }
        }

        static string LoadData()
        {
            return File.ReadLines("input.txt").First();
        }
        
        static void BuildMap(string data, Map map, int count, int currentDataPos, int currentX, int currentY)
        {
            var x = currentX;
            var y = currentY;
            var pos = currentDataPos;
            while (data[pos] != '$')
            {
                // Branch start
                if (data[pos] == '(')
                {
                    pos++;
                    BuildMap(data, map, count, pos, x, y);

                    while (!(data[pos] == ')' && count == 0))
                    {
                        if (data[pos] == '(')
                            count++;
                        else if (data[pos] == ')')
                            count--;

                        pos++;
                        if (count == 0 && data[pos - 1] == '|' && data[pos] != '|')
                            BuildMap(data, map, count, pos, x, y);
                    }
                    return;
                }
                else if (data[pos] == '|' || data[pos] == ')')
                {
                    // jump to end
                    while (!(data[pos] == ')' && count == 0))
                    {
                        if (data[pos] == '(')
                            count++;
                        else if (data[pos] == ')')
                            count--;
                        pos++;
                    }
                }
                else
                {
                    // navigate trougth the map
                    var item = data[pos];
                    var dx = 0;
                    var dy = 0;
                    if (item == 'N')
                        dy = -1;
                    else if (item == 'S')
                        dy = 1;
                    else if (item == 'W')
                        dx = -1;
                    else if (item == 'E')
                        dx = 1;

                    x += dx;
                    y += dy;
                    map.Data[x, y] = dx != 0 ? '|' : '-';
                    x += dx;
                    y += dy;
                    map.Data[x, y] = '.';
                }
                pos++;
            }
        }

        static void Dump(char[,] map)
        {
            var sb = new StringBuilder();
            for (var y = 0; y < map.GetLength(1); y++)
            {
                for (var x = 0; x < map.GetLength(0); x++)
                    sb.Append(map[x, y]);
                sb.AppendLine();
            }

            File.WriteAllText("out.txt", sb.ToString());
            Console.WriteLine(sb.ToString());
        }

        static string GetRoomLocation(string route)
        {
            var x = 0;
            var y = 0;
            foreach (var item in route)
            {
                if (item == 'N')
                    y--;
                else if (item == 'S')
                    y++;
                else if (item == 'W')
                    x--;
                else if (item == 'E')
                    x++;
            }
            return $"{x}/{y}";
        }

        static void Main(string[] args)
        {
            var data = LoadData();


            // var routes = new List<string>();
            // BuildRoute(data, routes);

            var map = new Map();
            BuildMap(data, map, 0, 1, map.StartX, map.StartY);

            Dump(map.Data);
        }
    }

}

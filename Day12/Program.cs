﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    class Program
    {
        class Tunnel
        {
            private StringBuilder buffer = new StringBuilder();
            public List<bool> Pods { get; set; }
            public int Position0 { get; set; }
            public int PodCount { get; set; }

            public Dictionary<string, bool> Rules { get; set; }

            public Tunnel(string initialState)
            {
                Pods = new List<bool>();
                var data = initialState.Substring(15);
                Position0 = data.Length + 1;
                PodCount = data.Length * 3;
                for (int i = 0; i < PodCount; i++)
                    Pods.Add(false);

                for (int i = 0; i < data.Length; i++)
                    Pods[Position0 + i] = data[i] == '#';

                Rules = new Dictionary<string, bool>();
            }

            public void AddRule(string data)
            {
                // ...## => #
                if (!string.IsNullOrEmpty(data) && data.EndsWith("#"))
                    Rules.Add(data.Substring(0, 5), true);
            }

            private string GetPattern(int podIndex)
            {
                buffer.Length = 0;
                for (int i = podIndex - 2; i <= podIndex + 2; i++)
                    if (Pods[i])
                        buffer.Append('#');
                    else
                        buffer.Append('.');
                return buffer.ToString();
            }

            public void AddGeneration()
            {
                var PodsNext = new List<bool>(PodCount);
                for (int i = 0; i < PodCount; i++)
                    PodsNext.Add(false);

                for (int i = 2; i < PodCount - 2; i++)
                {
                    var pattern = GetPattern(i);
                    bool plant;
                    if (Rules.TryGetValue(pattern, out plant))
                        PodsNext[i] = plant;
                }

                Pods = PodsNext;
            }

            public int Sum()
            {
                var result = 0;
                for (int i = 0; i < PodCount; i++)
                    if (Pods[i])
                        result += i - Position0;
                return result;
            }
        }

        static Tunnel LoadData()
        {
            var lines = File.ReadLines("input.txt").ToList();
            var result = new Tunnel(lines[0]);
            foreach (var item in lines.Skip(2))
                result.AddRule(item);

            return result;
        }

        static int Calculate(Tunnel data, long Generations)
        {
            for (long i = 0; i < Generations; i++)
            {
                data.AddGeneration();
                if (i % 10000 == 0)
                    Console.WriteLine(Generations - i);
            }
            var result = data.Sum();
            return result;
        }

        static void Main(string[] args)
        {
            var data = LoadData();

            Console.WriteLine($"Part1: {Calculate(data,20)}");
            Console.ReadLine();

            var now = DateTime.Now;

            // Console.WriteLine($"Part2: {Calculate(data, 50000000000)}");
            Console.WriteLine($"Part2: {Calculate(data, 50000)}");
            Console.WriteLine(DateTime.Now - now);
            
            Console.ReadLine();
        }
    }
}

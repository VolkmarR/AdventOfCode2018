using System;
using System.Collections;
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
            public BitArray Pods { get; set; }
            public int Position0 { get; set; }

            public Dictionary<int, bool> Rules { get; set; }

            private int MinRuleKey;

            public Tunnel(string initialState)
            {

                var data = initialState.Substring(15);
                Pods = new BitArray(data.Length + 10);
                Position0 = 5;

                for (int i = 0; i < data.Length; i++)
                    Pods[Position0 + i] = data[i] == '#';

                Rules = new Dictionary<int, bool>();
            }

            public void AddRule(string data)
            {
                int pattern = 0;
                if (!string.IsNullOrEmpty(data) && data.EndsWith("#"))
                {
                    foreach (var ch in data.Substring(0, 5))
                    {
                        pattern = pattern << 1;
                        if (ch == '#')
                            pattern++;
                    }

                    Rules.Add(pattern, true);
                    MinRuleKey = Rules.Keys.Min();
                }
            }

            private int GetPattern(int podIndex)
            {
                var result = 0;
                for (int i = podIndex - 2; i <= podIndex + 2; i++)
                {
                    result = result << 1;
                    if (Pods[i])
                        result++;
                }
                return result;
            }

            public bool AddGeneration()
            {
                var PodsNext = new BitArray(Pods.Length);
                for (int i = 2; i < Pods.Count - 2; i++)
                {
                    var pattern = GetPattern(i);
                    if (pattern >= MinRuleKey && Rules.ContainsKey(pattern))
                        PodsNext[i] = true;
                }

                // Check if the new Pods array is equal to the old one 
                // (Pattern match - Thanks to the hint from https://www.reddit.com/r/adventofcode/comments/a5eztl/2018_day_12_solutions/ebm4c9d)
                bool equal = true;
                for (int i = 2; i < Pods.Count - 2; i++)
                    equal = equal && (PodsNext[i + 1] == Pods[i]);

                Pods = PodsNext;
                if (GetPattern(Pods.Count - 3) > 0)
                    Pods.Length = Pods.Length + 1;

                return equal;
            }

            public long Sum()
            {
                long result = 0;
                for (int i = 0; i < Pods.Count; i++)
                    if (Pods[i])
                        result += (i - Position0);
                return result;
            }

            public void Dump()
            {
                Console.Write($"{Position0} : ");

                for (int i = 0; i < Pods.Count; i++)
                    Console.Write(Pods[i] ? '#' : '.');

                Console.WriteLine();
                Console.WriteLine();
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

        static long Calculate(Tunnel data, long Generations)
        {
            long lastSum = 0;
            for (long i = 1; i <= Generations; i++)
            {
                if (data.AddGeneration())
                {
                    // if a pattern was recognized, then calc the result buy adding the difference between the last and the current sum
                    // multiplied by the reaining solutions
                    var sum = data.Sum();
                    return sum + (sum - lastSum) * (Generations - i);
                }
                else
                    lastSum = data.Sum();
            }
            var result = data.Sum();
            return result;
        }

        static void Main(string[] args)
        {
            var data = LoadData();
            Console.WriteLine($"Part1: {Calculate(data, 20)}");

            data = LoadData();
            Console.WriteLine($"Part2: {Calculate(data, 50000000000)}");
        }
    }
}

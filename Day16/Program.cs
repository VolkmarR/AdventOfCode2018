using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    class Program
    {
        class Operation
        {
            private Action<int[], int[]> Action;

            public string Name { get; set; }

            public int[] Execute(int[] data, int[] operation)
            {
                var result = (int[])data.Clone();
                Action(result, operation);
                return result;
            }

            public Operation(string name, Action<int[], int[]> action)
            {
                Name = name;
                Action = action;
            }
        }

        class Sample
        {
            public int[] Before { get; set; }
            public int[] After { get; set; }
            public int[] Command { get; set; }

            public Sample(string before, string command, string after)
            {
                before = before.Substring(9, before.Length - 10);
                after = after.Substring(9, after.Length - 10);
                Before = before.Split(',').Select(q => Convert.ToInt32(q.Trim())).ToArray();
                After = after.Split(',').Select(q => Convert.ToInt32(q.Trim())).ToArray();
                Command = command.Split(' ').Select(q => Convert.ToInt32(q.Trim())).ToArray();
            }
        }

        static List<Operation> Operations = new List<Operation>
        { 
            // addr (add register) stores into register C the result of adding register A and register B.
            new Operation("addr", (d, o) => d[o[3]] = d[o[1]] + d[o[2]]),
            // addi (add immediate) stores into register C the result of adding register A and value B.
            new Operation("addi", (d, o) => d[o[3]] = d[o[1]] + o[2]),
            // mulr (multiply register) stores into register C the result of multiplying register A and register B.
            new Operation("mulr", (d, o) => d[o[3]] = d[o[1]] * d[o[2]]),
            // muli (multiply immediate) stores into register C the result of multiplying register A and value B.
            new Operation("muli", (d, o) => d[o[3]] = d[o[1]] * o[2]),
            // banr (bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
            new Operation("banr", (d, o) => d[o[3]] = d[o[1]] & d[o[2]]),
            // bani (bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.
            new Operation("bani", (d, o) => d[o[3]] = d[o[1]] & o[2]),
            // borr (bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
            new Operation("borr", (d, o) => d[o[3]] = d[o[1]] | d[o[2]]),
            // bori (bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B. 
            new Operation("bori", (d, o) => d[o[3]] = d[o[1]] | o[2]),
            // setr (set register) copies the contents of register A into register C. (Input B is ignored.)
            new Operation("setr", (d, o) => d[o[3]] = d[o[1]]),
            // seti (set immediate) stores value A into register C. (Input B is ignored.)
            new Operation("seti", (d, o) => d[o[3]] = o[1]),
            // gtir (greater-than immediate/register) sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0.
            new Operation("gtir", (d, o) => d[o[3]] = o[1] > d[o[2]] ? 1 : 0),
            // gtri (greater-than register/immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
            new Operation("gtri", (d, o) => d[o[3]] = d[o[1]] > o[2] ? 1 : 0),
            // gtrr (greater-than register/register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.
            new Operation("gtrr", (d, o) => d[o[3]] = d[o[1]] > d[o[2]] ? 1 : 0),
            // eqir (equal immediate/register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
            new Operation("eqir", (d, o) => d[o[3]] = o[1] == d[o[2]] ? 1 : 0),
            // eqri (equal register/immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
            new Operation("eqri", (d, o) => d[o[3]] = d[o[1]] == o[2] ? 1 : 0),
            // eqrr (equal register/register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
            new Operation("eqrr", (d, o) => d[o[3]] = d[o[1]] == d[o[2]] ? 1 : 0),
        };

        static Dictionary<int, Operation> Map = new Dictionary<int, Operation>();
        static List<Sample> Samples;
        static List<int[]> Prog;

        static void LoadData()
        {
            Samples = new List<Sample>();
            Prog = new List<int[]>();

            var data = File.ReadAllLines("input.txt");
            var i = 0;
            while (i < data.Length && data[i].StartsWith("Before"))
            {
                Samples.Add(new Sample(data[i], data[i + 1], data[i + 2]));
                i += 4;
            }

            while (i < data.Length)
            {
                if (!String.IsNullOrEmpty(data[i]))
                    Prog.Add(data[i].Split(' ').Select(q => Convert.ToInt32(q)).ToArray());
                i++;
            }
        }

        static List<Operation> CheckOperations(Sample sample)
        {
            var result = new List<Operation>();
            foreach (var item in Operations)
                if (item.Execute(sample.Before, sample.Command).SequenceEqual(sample.After))
                    result.Add(item);
            return result;
        }

        static int CalculatePart1()
        {
            var i = 0;
            foreach (var item in Samples)
                if (CheckOperations(item).Count >= 3)
                    i++;

            return i;
        }


        static int CalculatePart2()
        {
            var map = new Dictionary<int, Dictionary<Operation, int>>();
            foreach (var item in Samples)
            {
                if (!map.ContainsKey(item.Command[0]))
                    map[item.Command[0]] = new Dictionary<Operation, int>();

                foreach (var op in CheckOperations(item))
                {
                    if (!map[item.Command[0]].ContainsKey(op))
                        map[item.Command[0]][op] = 0;

                    map[item.Command[0]][op] += 1;

                }
            }

            while (map.Count > 0)
            {
                var el = map.OrderBy(q => q.Value.Count).FirstOrDefault();
                if (el.Value.Count == 1)
                {
                    var op = el.Value.First().Key;
                    Map[el.Key] = op;

                    map.Remove(el.Key);

                    foreach (var item in map)
                        item.Value.Remove(op);
                }
                else
                    throw new Exception("No operateion found");
            }


            var data = new int[] { 0, 0, 0, 0 };
            foreach (var item in Prog)
                data = Map[item[0]].Execute(data, item);

            return data[0];

        }



        static void Main(string[] args)
        {
            LoadData();

            Console.WriteLine(CalculatePart1());

            Console.WriteLine(CalculatePart2());
        }
    }
}

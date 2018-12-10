using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Solution
{
    class Graph
    {
        public Dictionary<char, List<char>> FollowedBy { get; private set; }
        public Dictionary<char, List<char>> DependsOn { get; private set; }
        public List<char> Start { get; private set; }

        private void Add(Dictionary<char, List<char>> index, char key, char? value)
        {
            if (!index.ContainsKey(key))
                index[key] = new List<char>();
            if (value.HasValue)
                index[key].Add(value.Value);
        }

        public Graph(List<string> data)
        {
            var used = new HashSet<char>();
            FollowedBy = new Dictionary<char, List<char>>();
            DependsOn = new Dictionary<char, List<char>>();
            foreach (var item in data)
            {
                var Before = item[5];
                var After = item[36];

                Add(FollowedBy, Before, After);
                Add(FollowedBy, After, null);
                Add(DependsOn, After, Before);
                used.Add(After);
            }

            Start = FollowedBy.Keys.Where(q => !used.Contains(q)).OrderBy(q => q).ToList();
        }
    }

    class Work
    {
        public char Step { get; set; }
        public int Until { get; set; }

        public Work(char on, int clock)
        {
            Step = on;
            Until = clock + ((int)on - (int)'A' + 1) + 60;
        }
    }

    static List<string> LoadData()
    {
        return File.ReadLines("input.txt").ToList();
    }

    static bool CanUseStep(char step, Graph data, HashSet<char> done)
    {
        // if the step is alreathy done, then it can not be used again
        if (done.Contains(step))
            return false;

        // if not all the steps that the step depends one are done, then the step can not be used
        if (data.DependsOn.ContainsKey(step))
            foreach (var before in data.DependsOn[step])
                if (!done.Contains(before))
                    return false;

        return true;
    }

    static void GetNextSteps(char step, Graph data, HashSet<char> done, ref List<char> nextSteps)
    {
        if (CanUseStep(step, data, done))
        {
            if (!nextSteps.Contains(step))
                nextSteps.Add(step);
        }
        else
            foreach (var newStart in data.FollowedBy[step])
                GetNextSteps(newStart, data, done, ref nextSteps);
    }

    static List<char> GetNextSteps(Graph data, HashSet<char> done)
    {
        var result = new List<char>();
        foreach (var step in data.Start)
            GetNextSteps(step, data, done, ref result);
        result.Sort();
        return result;
    }

    static string CalculatePart1(Graph data)
    {
        string result = "";
        var done = new HashSet<Char>();

        while (done.Count < data.FollowedBy.Count)
        {
            // Get the next step
            var next = GetNextSteps(data, done).First();
            // add the step to the resultstring and the done-list
            result += next;
            done.Add(next);
        }
  
        return result;
    }

    static int CalculatePart2(Graph data)
    {
        var WorkerCount = 5;
        var clock = 0;
        var done = new HashSet<Char>();

        var works = new Dictionary<int, Work>();
        var assignNewWork = true;

        while (done.Count < data.FollowedBy.Count)
        {
            // Check if work is finished
            for (var i = 0; i < WorkerCount; i++)
                if (works.TryGetValue(i, out var work))
                    if (work.Until == clock)
                    {
                        // Add the Step to the Done List and Remove the Work Element
                        done.Add(work.Step);
                        works.Remove(i);

                        assignNewWork = true;
                    };

            if (assignNewWork)
            {
                assignNewWork = false;
                var candidates = GetNextSteps(data, done);

                // remove all the steps that are currently worked on
                candidates.RemoveAll(q => works.Values.Any(s => s.Step == q));

                // Assign the next steps to free workers
                for (var i = 0; i < WorkerCount; i++)
                    if (!works.ContainsKey(i) && candidates.Count > 0)
                    {
                        works[i] = new Work(candidates.First(), clock);
                        candidates.RemoveAt(0);
                    }
            }
            clock++;
        }

        return clock - 1;
    }

    static void Main(string[] args)
    {
        var data = new Graph(LoadData());

        Console.WriteLine($"Part1: {CalculatePart1(data)}");

        Console.WriteLine($"Part2: {CalculatePart2(data)}");

    }
}

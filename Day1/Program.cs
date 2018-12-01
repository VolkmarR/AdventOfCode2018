using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day1_1
{
    class Program
    {
        // Load data form Inputfile
        static List<int> LoadData()
        {
            var data = new List<int>();
            foreach (var line in File.ReadLines("input.txt"))
            {
                int value;
                if (int.TryParse(line, out value))
                    data.Add(value);
            }

            return data;
        }

        // Calclate frequency
        static int CalculateFrequency(List<int> data)
        {
            return data.Sum();
        }

        // find the first frequency, that is calculated twice
        static int FindFrequencyCalculatedTwice(List<int> data)
        {
            var frequency = 0;
            var calculated = new HashSet<int>() { frequency };
            while (true)
                foreach (var change in data)
                {
                    frequency += change;
                    if (calculated.Contains(frequency))
                        return frequency;
                    calculated.Add(frequency);
                }
        }


        static void Main(string[] args)
        {
            var data = LoadData();

            // Calculate the Frequeny
            Console.WriteLine($"Freqency for Part 1: {CalculateFrequency(data)}");

            // Find the first Frequeny calculated twice
            Console.WriteLine($"Freqency for Part 2: {FindFrequencyCalculatedTwice(data)}");

        }
    }
}

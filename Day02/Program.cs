using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day2
{
    class Program
    {
        // Load the Input
        static List<string> LoadInput()
        {
            return File.ReadLines("input.txt").ToList();
        }

        // Calculate the Checksum for Part1
        static int CalcChecksum(List<string> data)
        {
            var letterCount = new Dictionary<char, int>();
            int charCount;
            var count2 = 0;
            var count3 = 0;
            foreach (var line in data)
            {
                // Count the letters using a Dictionary
                letterCount.Clear();
                foreach (var letter in line)
                    if (!letterCount.TryGetValue(letter, out charCount))
                        letterCount[letter] = 1;
                    else
                        letterCount[letter] = charCount + 1;

                // Increment the Counts, if any letter was fount 2 or 3 times
                if (letterCount.Values.Any(q => q == 2))
                    count2++;
                if (letterCount.Values.Any(q => q == 3))
                    count3++;
            }

            return count2 * count3;
        }

        static string FindFirstItemWithOneDifferentLetter(List<string> data)
        {
            // Start with the second Element of the List
            for (var i = 1; i < data.Count; i++)
            {
                var itemI = data[i];
                // Compare the current Element to all the prior Elements in the List
                for (var j = 0; j < i; j++)
                {
                    var itemJ = data[j];
                    if (itemI.Length != itemJ.Length)
                        continue;
                    var same = "";
                    var diff = 0;
                    // Compare the two Elements. Add all the same Letters to the Same String, count the Differences
                    for (var p = 0; p < itemI.Length; p++)
                    {
                        if (itemI[p] == itemJ[p])
                            same += itemI[p];
                        else
                            diff++;
                    }

                    if (diff == 1)
                        return same;
                }
            }

            return "";
        }

        static void Main(string[] args)
        {
            var data = LoadInput();

            Console.WriteLine(String.Format("Checksum Part 1: {0}", CalcChecksum(data)));

            Console.WriteLine(String.Format("Checksum Part 2: {0}", FindFirstItemWithOneDifferentLetter(data)));
        }
    }
}

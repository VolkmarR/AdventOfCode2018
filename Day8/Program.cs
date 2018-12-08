using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day8
{
    class Program
    {
        class Node
        {
            public List<Node> Childs { get; set; }
            public List<int> Metadata { get; set; }

            public Node(Queue<int> data)
            {
                var childCount = data.Dequeue();
                var metadataCount = data.Dequeue();

                Childs = new List<Node>();
                for (int i = 0; i < childCount; i++)
                    Childs.Add(new Node(data));

                Metadata = new List<int>();
                for (int i = 0; i < metadataCount; i++)
                    Metadata.Add(data.Dequeue());
            }

            public int SumMetadata()
            {
                return Metadata.Sum() + Childs.Select(q => q.SumMetadata()).Sum();
            }

            public int SumSpecial()
            {
                if (Childs.Count == 0)
                    return Metadata.Sum();

                var result = 0;
                foreach (var index in Metadata)
                    if (index >= 1 && index <= Childs.Count)
                        result += Childs[index - 1].SumSpecial();
                return result;
            }
        }

        private static List<int> LoadData()
        {
            return File.ReadLines("Input.txt").First().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(q => Convert.ToInt32(q)).ToList();
        }

        private static int CalculatePart1(Node data)
        {
            return data.SumMetadata();
        }

        private static int CalculatePart2(Node data)
        {
            return data.SumSpecial();
        }

        static void Main(string[] args)
        {
            var data = LoadData();

            Console.WriteLine($"Part1: {CalculatePart1(new Node(new Queue<int>(data)))}");

            Console.WriteLine($"Part2: {CalculatePart2(new Node(new Queue<int>(data)))}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day14
{
    class Program
    {
        class Game
        {
            public List<int> Scoreboard;
            public int[] CurrentRecipe;

            private int NewPosition(int current)
            {
                var newPos = current + Scoreboard[current] + 1;
                while (newPos >= Scoreboard.Count)
                    newPos -= Scoreboard.Count;
                return newPos;
            }

            public void AddRecipes()
            {
                var sum = CurrentRecipe.Sum(q => Scoreboard[q]);
                if (sum < 10)
                    Scoreboard.Add(sum);
                else
                {
                    Scoreboard.Add(sum / 10);
                    Scoreboard.Add(sum % 10);
                }

                for (int i = 0; i < CurrentRecipe.Length; i++)
                    CurrentRecipe[i] = NewPosition(CurrentRecipe[i]);
            }

            public long GetNumberAfterTrials(int trials)
            {
                while (Scoreboard.Count < 11 + trials)
                    AddRecipes();

                long result = 0;
                for (int i = trials; i < trials + 10; i++)
                {
                    result *= 10;
                    result += Scoreboard[i];
                }

                return result;
            }

            public long GetTrials(string startDigits)
            {
                var len = startDigits.Length;
                var cmp = Convert.ToInt32(startDigits);

                var result = 0;
                while (true)
                {
                    while (result + 10 < Scoreboard.Count)
                    {
                        
                        int sum = 0;
                        for (int i = result; i < result + len; i++)
                        {
                            sum *= 10;
                            sum += Scoreboard[i];
                        }

                        if (sum == cmp)
                            return result;

                        result++;
                    }

                    AddRecipes();
                }
            }

            public Game(params int[] recipes)
            {
                Scoreboard = new List<int>(recipes);
                CurrentRecipe = new int[recipes.Length];
                for (var i = 0; i < recipes.Length; i++)
                    CurrentRecipe[i] = i;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine(string.Format("Part 1: {0:0000000000}", new Game(3, 7).GetNumberAfterTrials(554401)));
            Console.WriteLine(string.Format("Part 2: {0}", new Game(3, 7).GetTrials("554401")));
        }
    }
}

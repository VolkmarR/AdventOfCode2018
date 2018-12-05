using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day4
{
    class Program
    {
        enum LineAction { BeginShift, FallsAsleep, WakesUp }

        // Class to Parse the Line
        class LineValues
        {
            public DateTime TimeStamp { get; private set; }
            public int ID { get; private set; }
            public LineAction Action { get; private set; }

            public LineValues(string line)
            {
                TimeStamp = DateTime.ParseExact(line.Substring(1, 16), "yyyy-MM-dd HH':'mm", CultureInfo.InvariantCulture);
                var action = line.Substring(19).Split(new Char[] { '#', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (action[0] == "Guard")
                {
                    ID = Convert.ToInt32(action[1]);
                    Action = LineAction.BeginShift;
                }
                else if (action[0] == "falls")
                    Action = LineAction.FallsAsleep;
                else if (action[0] == "wakes")
                    Action = LineAction.WakesUp;
            }
        }

        static List<LineValues> LoadData()
        {
            return File.ReadLines("input.txt").Select(q => new LineValues(q)).ToList();
        }

        static Dictionary<int, Dictionary<int, int>> BuildCalenders(List<LineValues> data)
        {
            var result = new Dictionary<int, Dictionary<int, int>>();

            Dictionary<int, int> currentCalender = null;
            var lastAsleep = false;
            var lastMinute = 0;
            foreach (var line in data.OrderBy(q => q.TimeStamp))
            {
                int newMinute = line.TimeStamp.Minute;
                // add the time period to the current calender, when the last action was Asleep
                if (currentCalender != null && lastAsleep)
                    for (int i = lastMinute; i < newMinute; i++)
                    {
                        int count;
                        if (!currentCalender.TryGetValue(i, out count))
                            currentCalender[i] = 1;
                        else
                            currentCalender[i] = count + 1;
                    }

                if (line.Action == LineAction.BeginShift)
                    if (!result.TryGetValue(line.ID, out currentCalender))
                    {
                        currentCalender = new Dictionary<int, int>();
                        result[line.ID] = currentCalender;
                    }
                lastAsleep = (line.Action == LineAction.FallsAsleep);
                lastMinute = newMinute;
            }

            return result;
        }

        static int CalculatePart1(List<LineValues> data)
        {
            var calenders = BuildCalenders(data);

            // Find the guard which sleeps the most
            var maxGuardID = 0;
            var maxSleep = 0;
            foreach (var calender in calenders)
            {
                var sleep = calender.Value.Sum(q => q.Value);
                if (sleep > maxSleep)
                {
                    maxSleep = sleep;
                    maxGuardID = calender.Key;
                }
            }

            // Find the minute with the higest count
            var item = calenders[maxGuardID].OrderByDescending(q => q.Value).First();

            return maxGuardID * item.Key;
        }

        static int CalculatePart2(List<LineValues> data)
        {
            var calenders = BuildCalenders(data);

            // Find the guard and the minute 
            var maxGuardID = 0;
            var maxMinute = 0;
            var maxMinuteCount = 0;
            foreach (var calender in calenders)
            {
                var maxSleepMinute = calender.Value.OrderByDescending(q => q.Value).FirstOrDefault();
                if (maxSleepMinute.Value > maxMinuteCount)
                {
                    maxGuardID = calender.Key;
                    maxMinute = maxSleepMinute.Key;
                    maxMinuteCount = maxSleepMinute.Value;
                }
            }

            return maxGuardID * maxMinute;
        }

        static void Main(string[] args)
        {
            var data = LoadData();

            Console.WriteLine($"Part1: {CalculatePart1(data)}");

            Console.WriteLine($"Part2: {CalculatePart2(data)}");
        }
    }
}

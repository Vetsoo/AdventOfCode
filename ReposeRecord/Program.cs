using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReposeRecord
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to REPOSE RECORD!");
            Console.WriteLine("");
            Console.WriteLine("Started reading input...");

            var guardData = File.ReadAllLines(@"Input/input.txt");
            var guardInformationData = guardData.Select(x => new GuardInformation() { TimeStamp = Convert.ToDateTime(x.Substring(1, x.LastIndexOf(']') - 1)), Information = x.Substring(x.LastIndexOf(']') + 2) });
            var orderedData = guardInformationData.OrderBy(x => x.TimeStamp);

            Console.WriteLine("Started calculating sleep times...");

            var listOfGuards = new List<Guard>();

            Guard guard = null;
            var minuteFellAsleep = 0;
            foreach (var guardInformation in orderedData)
            {
                if (guardInformation.Information.Contains('#'))
                {
                    var id = guardInformation.Information.Split(' ').FirstOrDefault(x => x.StartsWith('#'));
                    if (listOfGuards.All(x => x.Id != id))
                        listOfGuards.Add(new Guard() { Id = id });
                    guard = listOfGuards.FirstOrDefault(x => x.Id == id);
                }
                else
                {
                    if (guardInformation.Information.Contains("falls asleep"))
                    {
                        minuteFellAsleep = guardInformation.TimeStamp.Minute;
                    }
                    else if (guardInformation.Information.Contains("wakes up"))
                    {
                        guard.TotalTimeAsleep += guardInformation.TimeStamp.Minute - minuteFellAsleep;
                        for (var i = minuteFellAsleep; i < guardInformation.TimeStamp.Minute; i++)
                        {
                            guard.MinutesAsleep.Add(i);
                        }
                    }
                }
            }

            var guardAsleep = listOfGuards.OrderByDescending(x => x.TotalTimeAsleep).FirstOrDefault();
            var timeMostAsleep = guardAsleep.MinutesAsleep.GroupBy(x => x).OrderByDescending(x => x.Count()).Select(x => x.Key).FirstOrDefault();
            Console.WriteLine($"Guard {guardAsleep.Id}: Asleep for {guardAsleep.TotalTimeAsleep} minutes. Most asleep during minute {timeMostAsleep}. Answer: {Convert.ToInt32(guardAsleep.Id.Substring(1)) * timeMostAsleep}");

            Console.WriteLine("Started calculating minute most frequently asleep ...");

            var mostMinutesPerGuard = listOfGuards
                .Select(x => new Tuple<string, int, int>(
                    x.Id, 
                    x.MinutesAsleep
                        .GroupBy(y => y)
                        .OrderByDescending(z => z.Count())
                        .Select(a => a.Key)
                        .FirstOrDefault(), 
                    x.MinutesAsleep
                        .GroupBy(y => y)
                        .OrderByDescending(z => z.Count())
                        .Select(a => a.Count())
                        .FirstOrDefault()))
                .OrderByDescending(b => b.Item3)
                .FirstOrDefault();

            Console.WriteLine($"Guard {mostMinutesPerGuard.Item1} has been asleep most on minute {mostMinutesPerGuard.Item2}. Answer: {Convert.ToInt32(mostMinutesPerGuard.Item1.Substring(1)) * mostMinutesPerGuard.Item2}");

            Console.ReadKey();

        }
    }
}

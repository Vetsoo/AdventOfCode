using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SumOfItParts
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to THE SUM OF ITS PARTS!");
            Console.WriteLine("");
            Console.WriteLine("Started reading input...");

            var stepsText = File.ReadAllLines(@"Input/input.txt");

            Console.WriteLine("Started checking steps...");

            Regex inputRx = new Regex(@"Step ([A-Z]) must be finished before step ([A-Z]) can begin.", RegexOptions.Compiled);
            var output = new HashSet<string>();

            var steps = stepsText.Select(s => inputRx.Matches(s).Select(m => m.Groups).First())
                             .Select(s => (s[2].Value, s[1].Value))
                             .GroupBy(g => g.Item1)
                             .Select(s => new
                             {
                                 s.Key,
                                 dependencies = s.Select(s2 => s2.Item2)
                                                                      .OrderBy(o => o)
                             })
                             .ToDictionary(k => k.Key, v => v.dependencies.ToList());

            var steps2 = stepsText.Select(s => inputRx.Matches(s).Select(m => m.Groups).First())
                                .Select(s => (s[1].Value, s[2].Value))
                                .GroupBy(g => g.Item1)
                                .Select(s => new { s.Key, Values = s.Select(s2 => steps[s2.Item2]) })
                                .ToDictionary(k => k.Key, v => v.Values.ToList());

            steps = steps.Concat(steps2.Where(x => !steps.ContainsKey(x.Key)).ToDictionary(k => k.Key, v => new List<string>()))
                 .OrderBy(o => o.Key)
                 .ToDictionary(k => k.Key, v => v.Value);

            while (steps.Count > 0)
            {
                var key = steps.Where(x => x.Value.Count == 0).First().Key;
                if (steps2.ContainsKey(key))
                    foreach (var step2 in steps2[key])
                    {
                        step2.Remove(key);
                    }
                output.Add(key);
                steps.Remove(key);

            }

            Console.WriteLine(output.Aggregate("", (acc, s) => acc + s));

            p2(stepsText, inputRx, (time, letter) => time + letter - 4);

            Console.ReadKey();
        }

        static void p2(string[] stepsText, Regex inputRx, Func<int, int, int> calcTime)
        {
            int numWorkers = 5;
            var stepsP = stepsText.Select(s => inputRx.Matches(s).Select(m => m.Groups).First())
                             .Select(s => (s[2].Value[0], s[1].Value[0]))
                             .GroupBy(g => g.Item1)
                             .Select(s => new
                             {
                                 s.Key,
                                 dependencies = s.Select(s2 => s2.Item2)
                                                                      .OrderBy(o => o)
                             })
                             .ToDictionary(k => k.Key, v => v.dependencies.ToList());

            var stepsP2 = stepsText.Select(s => inputRx.Matches(s).Select(m => m.Groups).First())
                                .Select(s => (s[1].Value[0], s[2].Value[0]))
                                .GroupBy(g => g.Item1)
                                .Select(s => new { s.Key, Values = s.Select(s2 => stepsP[s2.Item2]) })
                                .ToDictionary(k => k.Key, v => v.Values.ToList());

            stepsP = stepsP.Concat(stepsP2.Where(x => !stepsP.ContainsKey(x.Key)).ToDictionary(k => k.Key, v => new List<char>()))
                 .OrderBy(o => o.Key)
                 .ToDictionary(k => k.Key, v => v.Value);

            var workers = Enumerable.Range(0, numWorkers).Select(s => new { wait = 0, letter = '\0' });
            var time = 0;

            while (stepsP.Count > 0)
            {
                var toProcess = workers.Where(x => x.wait == 0).Zip(stepsP.Where(x => x.Value.Count == 0), (worker, step) => (worker, step)).ToList();

                if (toProcess.Count() == 0)
                {
                    time = workers.Where(x => x.wait > 0).Select(s => s.wait).Min();
                    foreach (var worker in workers.Where(x => x.wait == time).ToList())
                    {
                        var key = worker.letter;
                        if (stepsP2.ContainsKey(key))
                            foreach (var step2 in stepsP2[key])
                            {
                                step2.Remove(key);
                            }
                    }
                    workers = workers.Where(x => x.wait > time);
                }
                else
                {
                    foreach (var key in toProcess.Select(s => s.Item2.Key))
                    {
                        stepsP.Remove(key);
                    }
                    workers = workers.Where(x => x.wait > 0).Concat(toProcess.Select(p => new { wait = calcTime(time, p.Item2.Key), letter = p.Item2.Key }));
                }
                workers = workers.Concat(Enumerable.Range(0, numWorkers - workers.Count()).Select(s => new { wait = 0, letter = '\0' })).ToList();
            }

            Console.WriteLine(workers.First().wait);
        }
    }
}

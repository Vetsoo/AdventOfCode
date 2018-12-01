using System;
using System.Collections.Generic;

namespace FrequencyCalculator
{
    class Program
    {
        public static int newFrequency = 0;
        public static bool completed = false;
        static void Main(string[] args)
        {
            var startFrequency = 0;
            var uniqueFrequencies = new List<int>();
            newFrequency = startFrequency;

            Console.WriteLine("Welcome to the FREQUENCY CALCULATOR!");
            Console.WriteLine("");
            Console.WriteLine("Started reading input...");
            var frequencyChanges = System.IO.File.ReadAllLines(@"Input/input.txt");
            Console.WriteLine("Started calculating new frequency...");
            while (!completed)
            {
                LoopOverInput(frequencyChanges, uniqueFrequencies);
                Console.WriteLine("Looped the list");
            }
            Console.WriteLine($"NEW FREQUENCY = {newFrequency}");
            Console.ReadKey();
        }

        private static void LoopOverInput(string[] frequencyChanges, List<int> uniqueFrequencies)
        {
            foreach (var frequencyChange in frequencyChanges)
            {
                newFrequency += Convert.ToInt32(frequencyChange);
                if (!uniqueFrequencies.Contains(newFrequency))
                    uniqueFrequencies.Add(newFrequency);
                else
                {
                    completed = true;
                    break;
                }
            }
        }
    }
}

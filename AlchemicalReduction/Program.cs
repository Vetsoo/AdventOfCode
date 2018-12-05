using System;
using System.IO;

namespace AlchemicalReduction
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to ALCHEMICAL REDUCTION!");
            Console.WriteLine("");
            Console.WriteLine("Started reading input...");

            var orinigalPolymer = File.ReadAllText(@"Input/input.txt");

            Console.WriteLine("Started reaction of the units...");

            var shortestPolymer = orinigalPolymer.Length;
            var isReactionComplete = false;
            var keepGoing = false;
            var index = 0;
            for (int i = 1; i < 27; i++)
            {
                var polymer = orinigalPolymer;
                var charToRemove = Number2String(i);
                Console.WriteLine($"Character to remove: {charToRemove}");
                polymer = polymer.Replace(charToRemove, string.Empty);
                polymer = polymer.Replace(charToRemove.ToLower(), string.Empty);
                while (!isReactionComplete)
                {
                    try
                    {
                        var unit = polymer.Substring(index, 1);
                        var followingUnit = polymer.Substring(index + 1, 1);

                        if (unit.ToLower() == followingUnit.ToLower())
                        {
                            if (char.IsLower(char.Parse(unit)) && char.IsUpper(char.Parse(followingUnit)) ||
                                char.IsLower(char.Parse(followingUnit)) && char.IsUpper(char.Parse(unit)))
                            {
                                keepGoing = true;
                                polymer = polymer.Remove(index, 2);
                            }
                            else
                                index++;
                        }
                        else
                            index++;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        index = 0;
                        if (keepGoing)
                            keepGoing = false;
                        else
                            isReactionComplete = true;
                    }
                }

                if (shortestPolymer > polymer.Trim().Length)
                    shortestPolymer = polymer.Trim().Length;

                Console.WriteLine($"Final polymer: {polymer.Trim().Length}");
                Console.WriteLine("");
                isReactionComplete = false;
            }

            Console.WriteLine($"Shortest polymer possible: {shortestPolymer}");
            Console.ReadKey();
        }

        private static string Number2String(int number)
        {
            var c = (char)(65 + (number - 1));
            return c.ToString();
        }
    }
}

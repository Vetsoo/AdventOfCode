using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var doubleLetters = 0;
            var tripleLetters = 0;

            Console.WriteLine("Welcome to the INVENTORY MANAGEMENT SYSTEM!");
            Console.WriteLine("");
            Console.WriteLine("Started reading input...");
            var boxIds = System.IO.File.ReadAllLines(@"Input/input.txt");
            Console.WriteLine("Started checking box IDs...");

            foreach(var id in boxIds)
            {
                var letters = new List<CheckList>();
                
                foreach(var letter in id)
                {
                    var isFound = false;
                    foreach (var l in letters)
                    {
                        if (l.Letter == letter)
                        {
                            isFound = true;
                            l.Occurences++;
                        }
                    }
                    if (!isFound)
                        letters.Add(new CheckList() { Letter = letter, Occurences = 1 });
                }

                if (letters.Any(x => x.Occurences == 2))
                    doubleLetters++;
                if (letters.Any(x => x.Occurences == 3))
                    tripleLetters++;
            }

            Console.WriteLine("Calculating checksum...");
            var checkSum = doubleLetters * tripleLetters;
            Console.WriteLine($"{doubleLetters} * {tripleLetters} = {checkSum}");

            var uniqueWords = new List<string>();

            foreach (var id in boxIds)
            {
                if (uniqueWords.Count == 0)
                    uniqueWords.Add(id);
                else
                {
                    var wordLength = id.Length;
                    var numberOfDifferences = 0;
                    var commonLetters = string.Empty;
                    foreach (var uniqueWord in uniqueWords)
                    {
                        commonLetters = string.Empty;
                        numberOfDifferences = 0;
                        for (var i = 0; i < wordLength; i++)
                        {
                            if (id[i] != uniqueWord[i])
                            {
                                numberOfDifferences++;
                                if (numberOfDifferences > 1)
                                    break;
                            } else
                            {
                                commonLetters += id[i];
                            }
                        }

                        if (numberOfDifferences <= 1)
                        {
                            Console.WriteLine(id);
                            Console.WriteLine(commonLetters);
                        }
                    }
                    uniqueWords.Add(id);
                }
            }

            Console.ReadKey();
        }
    }

    public class CheckList
    {
        public char Letter { get; set; }
        public int Occurences { get; set; }
    }
}

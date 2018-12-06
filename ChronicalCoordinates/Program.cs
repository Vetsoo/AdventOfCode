using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChronicalCoordinates
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to CHRONICAL COORDINATES");
            Console.WriteLine("");
            Console.WriteLine("Started reading input...");

            var inputList = File.ReadAllLines(@"Input/input.txt");

            Console.WriteLine("Started calculating closest coordinates...");

            var coords = Parse(inputList);

            var minX = coords.Min(coord => coord.x) - 1;
            var maxX = coords.Max(coord => coord.x) + 1;
            var minY = coords.Min(coord => coord.y) - 1;
            var maxY = coords.Max(coord => coord.y) + 1;

            var area = new int[coords.Length];

            foreach (var x in Enumerable.Range(minX, maxX - minX + 1))
            {
                foreach (var y in Enumerable.Range(minY, maxY - minX + 1))
                {
                    var d = coords.Select(coord => Dist((x, y), coord)).Min();
                    var closest = Enumerable.Range(0, coords.Length).Where(i => Dist((x, y), coords[i]) == d).ToArray();

                    if (closest.Length != 1)
                    {
                        continue;
                    }

                    if (x == minX || x == maxX || y == minY || y == maxY)
                    {
                        foreach (var icoord in closest)
                        {
                            if (area[icoord] != -1)
                            {
                                area[icoord] = -1;
                            }
                        }
                    }
                    else
                    {
                        foreach (var icoord in closest)
                        {
                            if (area[icoord] != -1)
                            {
                                area[icoord]++;
                            }
                        }
                    }
                }
            }

            Console.WriteLine(area.Max());

            var partTwoArea = 0;
            foreach (var x in Enumerable.Range(minX, maxX - minX + 1))
            {
                foreach (var y in Enumerable.Range(minY, maxY - minX + 1))
                {
                    var d = coords.Select(coord => Dist((x, y), coord)).Sum();
                    if (d < 10000)
                        partTwoArea++;
                }
            }
            Console.WriteLine(partTwoArea);
            Console.ReadKey();
        }

        static int Dist((int x, int y) c1, (int x, int y) c2)
        {
            return Math.Abs(c1.x - c2.x) + Math.Abs(c1.y - c2.y);
        }

        static (int x, int y)[] Parse(string[] input) => (
            from line in input
            let coords = line.Split(", ").Select(int.Parse).ToArray()
            select (coords[0], coords[1])
        ).ToArray();
    }
}

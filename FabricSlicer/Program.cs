using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FabricSlicer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the FABRIC SLICER!");
            Console.WriteLine("");
            Console.WriteLine("Started reading input...");
            var claims = File.ReadAllLines(@"Input/input.txt");
            var claimObjects = new List<Claim>();
            foreach (var claim in claims)
            {
                var splitClaim = claim.Split('#', ' ', ',', ':', 'x');
                claimObjects.Add(new Claim
                {
                    //#15 @ 839,821: 20x10
                    Id = splitClaim[1],
                    Left = Convert.ToInt32(splitClaim[3]),
                    Top = Convert.ToInt32(splitClaim[4]),
                    Width = Convert.ToInt32(splitClaim[6]),
                    Height = Convert.ToInt32(splitClaim[7])
                });
            }
            Console.WriteLine("Started calcing overlapping inches...");

            var testObjects = new List<Claim>()
            {
                new Claim
                {
                    Id = "1", Left = 1, Top = 3, Width = 4, Height = 4
                },
                                new Claim
                {
                    Id = "2", Left = 3, Top = 1, Width = 4, Height = 4
                },
                                                new Claim
                {
                    Id = "3", Left = 5, Top = 5, Width = 2, Height = 2
                }
            };

            var duplicateList = claimObjects.ToList();

            var totalOverlappingArea = 0;

            foreach (var orgClaim in claimObjects)
            {
                foreach (var claim in duplicateList)
                {
                    if (orgClaim.Id == claim.Id)
                        continue;

                    totalOverlappingArea += (Math.Min(orgClaim.BottomRight.Item1, claim.BottomRight.Item1) -
                        Math.Max(orgClaim.TopLeft.Item1, claim.TopLeft.Item1)) *
                        (Math.Min(orgClaim.BottomRight.Item2, claim.BottomRight.Item2) -
                        Math.Max(orgClaim.TopLeft.Item2, claim.TopLeft.Item2));
                }

                duplicateList.Remove(duplicateList.FirstOrDefault(x => x.Id == orgClaim.Id));
            }

            var answer = (
    from claim in Regex.Matches(File.ReadAllText("Input/input.txt"), @"(?<claim>#(?<id>\d+) @ (?<l>\d+),(?<t>\d+): (?<w>\d+)x(?<h>\d+))\n")
    let l = int.Parse(claim.Groups["l"].Value)
    let t = int.Parse(claim.Groups["t"].Value)
    let w = int.Parse(claim.Groups["w"].Value)
    let h = int.Parse(claim.Groups["h"].Value)
    from x in Enumerable.Range(l, w)
    from y in Enumerable.Range(t, h)
    group (x, y) by (x, y) into g
    where g.Count() > 1
    select g)
    .Count();

            var table = new int[1000, 1000];
            var noOverlaps = new List<int>();

            foreach (var claim in claims)
            {
                var parts = claim.Split(new[] { '#', '@', ' ', ',', ':', 'x' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                var id = parts[0];
                var x = parts[1];
                var y = parts[2];
                var w = parts[3];
                var h = parts[4];

                noOverlaps.Add(id);

                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < h; j++)
                    {
                        var previousId = table[x + i, y + j];
                        if (previousId == 0)
                        {
                            table[x + i, y + j] = id;
                        }
                        else
                        {
                            noOverlaps.Remove(id);
                            noOverlaps.Remove(previousId);
                        }
                    }
                }
            }

            var answer2 = noOverlaps.First();

            Console.WriteLine($"Total overlapping area: {answer}");
            Console.WriteLine($"Total overlapping area: {answer2}");
            Console.ReadKey();
        }
    }
}

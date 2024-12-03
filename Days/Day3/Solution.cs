using System.Text.RegularExpressions;
using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day3
{
    public class Solution : ISolver
    {
        public int SolvePart1(string[] input)
        {
            int total = 0;
            string pattern = @"mul\((\d+),(\d+)\)";
            
            foreach (var line in input)
            {
                MatchCollection matches = Regex.Matches(line, pattern);

                foreach (Match match in matches)
                {
                    int a = int.Parse(match.Groups[1].Value);
                    int b = int.Parse(match.Groups[2].Value);

                    total += a * b;
                }
            }
            return total;
        }

        public int SolvePart2(string[] input)
        {
            int total = 0;
            var pattern = @"mul\((\d{1,3}),(\d{1,3})\)";

            var doPattern = @"do\(\)";
            var dontPattern = @"don't\(\)";
            bool enabled = true;
            
            foreach (var line in input)
            {
                int pos = 0;

                while (pos < line.Length)
                {
                    Match doMatch = Regex.Match(line[pos..], doPattern);
                    if (doMatch.Success && doMatch.Index == 0)
                    {
                        enabled = true;
                        pos += doMatch.Length;
                        continue;
                    }
                    
                    Match dontMatch = Regex.Match(line[pos..], dontPattern);
                    if (dontMatch.Success && dontMatch.Index == 0)
                    {
                        enabled = false;
                        pos += dontMatch.Length;
                        continue;
                    }
                    
                    Match mulMatch = Regex.Match(line[pos..], pattern);
                    if (mulMatch.Success && mulMatch.Index == 0)
                    {
                        if (enabled)
                        {
                            int a = int.Parse(mulMatch.Groups[1].Value);
                            int b = int.Parse(mulMatch.Groups[2].Value);
                            total += a * b;
                        }
                        pos += mulMatch.Length;
                        continue;
                    }
                    
                    pos++;
                }
            }
            return total;
        }

        public static void Solve()
        {
            string inputPath = "Days/Day3/input.txt";
            string[] lines = File.ReadAllLines(inputPath);
            
            var solution = new Solution();
            
            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(lines));
        }
    }
}
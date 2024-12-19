using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day19
{
    public class Solution : ISolver
    {
        private static (HashSet<string> patterns, List<string> designs) ParseInput(string[] input)
        {
            var patterns = input[0].Split(", ").ToHashSet();
            var designs = input.Skip(2).ToList();
            return (patterns, designs);
        }

        private static bool CanConstruct(string design, HashSet<string> patterns)
        {
            var memo = new Dictionary<string, bool>();
            return CanConstructHelper(design, patterns, memo);
        }

        private static bool CanConstructHelper(string design, HashSet<string> patterns, Dictionary<string, bool> memo)
        {
            if (design == string.Empty) return true;
            if (memo.ContainsKey(design)) return memo[design];

            foreach (var pattern in patterns)
            {
                if (design.StartsWith(pattern))
                {
                    var remaining = design.Substring(pattern.Length);
                    if (CanConstructHelper(remaining, patterns, memo))
                    {
                        memo[design] = true;
                        return true;
                    }
                }
            }

            memo[design] = false;
            return false;
        }

        private static long CountWays(string design, HashSet<string> patterns)
        {
            var memo = new Dictionary<string, long>();
            return CountWaysHelper(design, patterns, memo);
        }

        private static long CountWaysHelper(string design, HashSet<string> patterns, Dictionary<string, long> memo)
        {
            if (design == string.Empty) return 1;
            if (memo.ContainsKey(design)) return memo[design];

            long totalWays = 0;

            foreach (var pattern in patterns)
            {
                if (design.StartsWith(pattern))
                {
                    var remaining = design.Substring(pattern.Length);
                    totalWays += CountWaysHelper(remaining, patterns, memo);
                }
            }

            memo[design] = totalWays;
            return totalWays;
        }
        
        public int SolvePart1(string[] input)
        {
            var (patterns, designs) = ParseInput(input);
            int count = 0;

            foreach (var design in designs)
            {
                if (CanConstruct(design, patterns))
                {
                    count++;
                }
            }

            return count;
        }

        public int SolvePart2(string[] input)
        {
            var (patterns, designs) = ParseInput(input);
            long totalWays = 0;

            foreach (var design in designs)
            {
                totalWays += CountWays(design, patterns);
            }

            Console.WriteLine($"Part 2 (Long): {totalWays}");
            return 0;
        }

        public static void Solve()
        {
            string inputPath = "Days/Day19/input.txt";
            string[] lines = File.ReadAllLines(inputPath);

            var solution = new Solution();
            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(lines));
        }
    }
}
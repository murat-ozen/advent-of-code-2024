using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day11
{
    public class Solution : ISolver
    {
        static long SimulateBlinks(List<string> stones, int blinks)
        {
            Dictionary<string, long> stoneCounts = new Dictionary<string, long>();

            foreach (string stone in stones)
            {
                if (!stoneCounts.ContainsKey(stone))
                    stoneCounts[stone] = 0;
                stoneCounts[stone]++;
            }

            for (int i = 0; i < blinks; i++)
            {
                Dictionary<string, long> newStoneCounts = new Dictionary<string, long>();

                foreach (var kvp in stoneCounts)
                {
                    string stone = kvp.Key;
                    long count = kvp.Value;

                    if (stone == "0")
                    {
                        AddStone(newStoneCounts, "1", count);
                    }
                    else if (stone.Length % 2 == 0)
                    {
                        int mid = stone.Length / 2;
                        string left = stone.Substring(0, mid).TrimStart('0');
                        string right = stone.Substring(mid).TrimStart('0');

                        AddStone(newStoneCounts, string.IsNullOrEmpty(left) ? "0" : left, count);
                        AddStone(newStoneCounts, string.IsNullOrEmpty(right) ? "0" : right, count);
                    }
                    else
                    {
                        long number = long.Parse(stone);
                        AddStone(newStoneCounts, (number * 2024).ToString(), count);
                    }
                }

                stoneCounts = newStoneCounts;

            }

            return GetTotalStoneCount(stoneCounts);
        }

        static void AddStone(Dictionary<string, long> stoneCounts, string stone, long count)
        {
            if (!stoneCounts.ContainsKey(stone))
                stoneCounts[stone] = 0;
            stoneCounts[stone] += count;
        }

        static long GetTotalStoneCount(Dictionary<string, long> stoneCounts)
        {
            long total = 0;
            foreach (var count in stoneCounts.Values)
            {
                total += count;
            }
            return total;
        }
        public int SolvePart1(string[] input)
        {
            List<string> stones = input[0].Split(" ").ToList();
            int blinks = 25;

            long result = SimulateBlinks(stones, blinks);

            return (int)result;
        }

        public int SolvePart2(string[] input)
        {
            List<string> stones = input[0].Split(" ").ToList();
            int blinks = 75;

            long result = SimulateBlinks(stones, blinks);
            
            Console.WriteLine($"Part 2 (Long): {result}");
            return (int)Math.Min(result, int.MaxValue);
        }

        public static void Solve()
        {
            string inputPath = "Days/Day11/input.txt";
            string[] lines = File.ReadAllLines(inputPath);

            var solution = new Solution();

            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2 (Max Integer): " + solution.SolvePart2(lines));
        }
    }
}
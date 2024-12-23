using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day22
{
    public class Solution : ISolver
    {
        private List<int> GeneratePrices(long initial)
        {
            var prices = new List<int>();
            long secret = initial;

            for (int i = 0; i <= 2000; i++)
            {
                prices.Add((int)(secret % 10));
                secret = MixAndPrune(secret, secret * 64);
                secret = MixAndPrune(secret, secret / 32);
                secret = MixAndPrune(secret, secret * 2048);
            }

            return prices;
        }

        private List<int> CalculateChanges(List<int> prices)
        {
            var changes = new List<int>();

            for (int i = 1; i < prices.Count; i++)
            {
                changes.Add(prices[i] - prices[i - 1]);
            }

            return changes;
        }

        private Dictionary<(int, int, int, int), int> GetPatternScores(List<int> prices, List<int> changes)
        {
            var scores = new Dictionary<(int, int, int, int), int>();

            for (int i = 0; i <= changes.Count - 4; i++)
            {
                var pattern = (changes[i], changes[i + 1], changes[i + 2], changes[i + 3]);

                if (!scores.ContainsKey(pattern))
                {
                    scores[pattern] = prices[i + 4];
                }
            }

            return scores;
        }


        private long GenerateNextSecret(long secret)
        {
            secret = MixAndPrune(secret, secret * 64);
            secret = MixAndPrune(secret, secret / 32);
            secret = MixAndPrune(secret, secret * 2048);
            return secret;
        }

        private long MixAndPrune(long secret, long value)
        {
            secret ^= value;
            secret %= 16777216;
            return secret;
        }
        
        public int SolvePart1(string[] input)
        {
            var initialNumbers = input.Select(long.Parse).ToArray();

            long sum = 0;
            foreach (var initial in initialNumbers)
            {
                long secret = initial;
                for (int i = 0; i < 2000; i++)
                {
                    secret = GenerateNextSecret(secret);
                }
                sum += secret;
            }

            Console.WriteLine($"Part 1 (Long): {sum}");
            return 0;
        }

        public int SolvePart2(string[] input)
        {
            var scoreMap = new Dictionary<(int, int, int, int), int>();

            foreach (var line in input)
            {
                long initialSecret = long.Parse(line);
                var prices = GeneratePrices(initialSecret);
                var changes = CalculateChanges(prices);
                var scores = GetPatternScores(prices, changes);

                foreach (var (pattern, value) in scores)
                {
                    if (scoreMap.ContainsKey(pattern))
                    {
                        scoreMap[pattern] += value;
                    }
                    else
                    {
                        scoreMap[pattern] = value;
                    }
                }
            }

            return scoreMap.Values.Max();
        }

        public static void Solve()
        {
            string inputPath = "Days/Day22/input.txt";
            string[] lines = File.ReadAllLines(inputPath);

            var solution = new Solution();
            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(lines));
        }
    }
}

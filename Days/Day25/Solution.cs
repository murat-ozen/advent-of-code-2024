using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day25
{
    public class Solution : ISolver
    {
        private (List<string[]> locks, List<string[]> keys) ParseInput(string[] input)
        {
            var locks = new List<string[]>();
            var keys = new List<string[]>();
            var currentSchematic = new List<string>();

            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (currentSchematic.Count > 0)
                    {
                        var isKey = currentSchematic[0].All(c => c == '.');

                        if (isKey)
                        {
                            keys.Add(currentSchematic.ToArray());
                        }
                        else
                        {
                            locks.Add(currentSchematic.ToArray());
                        }

                        currentSchematic.Clear();
                    }
                }
                else
                {
                    currentSchematic.Add(line);
                }
            }

            if (currentSchematic.Count > 0)
            {
                var isKey = currentSchematic[0].All(c => c == '.');

                if (isKey)
                {
                    keys.Add(currentSchematic.ToArray());
                }
                else
                {
                    locks.Add(currentSchematic.ToArray());
                }
            }

            return (locks, keys);
        }

        private bool Fits(string[] key, string[] lockSchematic)
        {
            int rows = key.Length;
            int cols = key[0].Length;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (key[r][c] == '#' && lockSchematic[r][c] == '#')
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public int SolvePart1(string[] input)
        {
            var (locks, keys) = ParseInput(input);
            int validPairs = 0;

            foreach (var key in keys)
            {
                foreach (var lockSchematic in locks)
                {
                    if (Fits(key, lockSchematic))
                    {
                        validPairs++;
                    }
                }
            }

            return validPairs;
        }

        public int SolvePart2(string[] input)
        {
            return 0;
        }

        public static void Solve()
        {
            string inputPath = "Days/Day25/input.txt";
            string[] lines = File.ReadAllLines(inputPath);

            var solution = new Solution();
            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(lines));
        }
    }
}
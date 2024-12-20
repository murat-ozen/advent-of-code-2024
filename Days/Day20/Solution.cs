using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day20
{
    public class Solution : ISolver
    {
        private Dictionary<(int x, int y), int> steps = new();
        private Dictionary<(int x, int y), char> map = new();
        private (int x, int y) start;
        private (int x, int y) end;

        private void BuildMapAndFindPath(string[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    map[(i, j)] = input[i][j];
                    if (input[i][j] == 'S') start = (i, j);
                    if (input[i][j] == 'E') end = (i, j);
                }
            }

            var current = start;
            steps[start] = 0;
            int stepCount = 0;

            do
            {
                current = GetNextLocation(current);
                steps[current] = ++stepCount;
            } while (current != end);
        }

        private (int x, int y) GetNextLocation((int x, int y) current)
        {
            var neighbors = GetNeighbors(current);
            return neighbors.FirstOrDefault(n => map.ContainsKey(n) && map[n] != '#' && !steps.ContainsKey(n));
        }

        private List<(int x, int y)> GetNeighbors((int x, int y) pos, int distance = 1)
        {
            var result = new List<(int x, int y)>();
            int[] dx = { -distance, distance, 0, 0 };
            int[] dy = { 0, 0, -distance, distance };

            for (int i = 0; i < 4; i++)
            {
                int newX = pos.x + dx[i];
                int newY = pos.y + dy[i];
                result.Add((newX, newY));
            }

            return result;
        }

        private int ManhattenDistance((int x, int y) a, (int x, int y) b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        public int SolvePart1(string[] input)
        {
            BuildMapAndFindPath(input);

            int savingsCount = 0;
            foreach (var (loc, s) in steps)
            {
                foreach (var neighbor in GetNeighbors(loc, distance: 2))
                {
                    if (steps.ContainsKey(neighbor))
                    {
                        int saved = steps[neighbor] - steps[loc] - 2;
                        if (saved >= 100) savingsCount++;
                    }
                }
            }

            return savingsCount;
        }

        public int SolvePart2(string[] input)
        {
            int savingsCount = 0;

            foreach ((var loc, var s) in steps)
            {
                foreach (var n in steps.Where(a => ManhattenDistance(a.Key, loc) <= 20))
                {
                    int saved = n.Value - steps[loc] - ManhattenDistance(n.Key, loc);
                    if (saved >= 100)
                    {
                        savingsCount++;
                    }
                }
            }

            return savingsCount;
        }

        public static void Solve()
        {
            string inputPath = "Days/Day20/input.txt";
            string[] lines = File.ReadAllLines(inputPath);

            var solution = new Solution();
            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(lines));
        }
    }
}
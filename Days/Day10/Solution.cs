using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day10
{
    public class Solution : ISolver
    {
        private static readonly (int dx, int dy)[] Directions =
        {
            (0, -1),
            (1, 0),
            (0, 1),
            (-1, 0)
        };

        private char[,] ParseMap(string[] input)
        {
            int rows = input.Length;
            int cols = input[0].Length;
            char[,] map = new char[rows, cols];

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    map[y, x] = input[y][x];
                }
            }

            return map;
        }

        private int ExploreTrail(char[,] grid, int startX, int startY)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            
            Queue<(int x, int y, int height)> queue = new();
            queue.Enqueue((startX, startY, 0));

            HashSet<(int x, int y)> visited = new();

            HashSet<(int, int)> reachableNines = new();

            while (queue.Count > 0)
            {
                var (x, y, height) = queue.Dequeue();

                foreach (var (dx, dy) in Directions)
                {
                    int nx = x + dx;
                    int ny = y + dy;

                    if (nx >= 0 && ny >= 0 && nx < cols && ny < rows &&
                        !visited.Contains((nx, ny)) &&
                        grid[ny, nx] - '0' == height + 1)
                    {
                        visited.Add((nx, ny));
                        if (grid[ny, nx] == '9')
                        {
                            reachableNines.Add((nx, ny));
                        }
                        else
                        {
                            queue.Enqueue((nx, ny, height + 1));
                        }
                    }
                }
            }

            return reachableNines.Count;
        }

        private int CalculateTrailheadRating(char[,] grid, int startX, int startY)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            int CountPaths(int x, int y, int height)
            {
                if (grid[y, x] == '9')
                    return 1;

                int totalPaths = 0;

                foreach (var (dx, dy) in Directions)
                {
                    int nx = x + dx;
                    int ny = y + dy;

                    if (nx >= 0 && ny >= 0 && nx < cols && ny < rows &&
                        grid[ny, nx] - '0' == height + 1)
                    {
                        totalPaths += CountPaths(nx, ny, height + 1);
                    }
                }

                return totalPaths;
            }

            return CountPaths(startX, startY, 0);
        }

        public (int, int) Total(string[] input, bool part2 = false)
        {
            char[,] grid = ParseMap(input);
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            int totalScore = 0;
            int totalRating = 0;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    if (grid[y, x] == '0')
                    {
                        if(!part2)
                        {
                            totalScore += ExploreTrail(grid, x, y);
                        }
                        else
                        {
                            totalRating += CalculateTrailheadRating(grid, x, y);
                        }
                        
                    }
                }
            }

            return (totalScore, totalRating);
        }

        public int SolvePart1(string[] input)
        {
            (int totalScore, _) = Total(input);
            return totalScore;
        }

        public int SolvePart2(string[] input)
        {
            (_, int totalRating) = Total(input, part2: true);
            return totalRating;
        }

        public static void Solve()
        {
            string inputPath = "Days/Day10/input.txt";
            string[] lines = File.ReadAllLines(inputPath);

            var solution = new Solution();

            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(lines));
        }
    }
}
using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day18
{
    public class Solution : ISolver
    {
        private int FindShortestPath(bool[,] grid, (int x, int y) start, (int x, int y) end)
        {
            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };
            var queue = new Queue<((int x, int y) pos, int steps)>();
            var visited = new bool[grid.GetLength(0), grid.GetLength(1)];
            queue.Enqueue((start, 0));
            visited[start.x, start.y] = true;

            while (queue.Count > 0)
            {
                var (current, steps) = queue.Dequeue();

                if (current == end)
                    return steps;

                for (int i = 0; i < 4; i++)
                {
                    int nx = current.x + dx[i];
                    int ny = current.y + dy[i];

                    if (nx >= 0 && ny >= 0 && nx < grid.GetLength(0) && ny < grid.GetLength(1) &&
                        !grid[nx, ny] && !visited[nx, ny])
                    {
                        queue.Enqueue(((nx, ny), steps + 1));
                        visited[nx, ny] = true;
                    }
                }
            }

            return -1;
        }
        
        private bool PathExists(bool[,] grid, (int x, int y) start, (int x, int y) end)
        {
            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };
            var queue = new Queue<(int x, int y)>();
            var visited = new bool[grid.GetLength(0), grid.GetLength(1)];

            queue.Enqueue(start);
            visited[start.x, start.y] = true;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (current == end)
                    return true;

                for (int i = 0; i < 4; i++)
                {
                    int nx = current.x + dx[i];
                    int ny = current.y + dy[i];

                    if (nx >= 0 && ny >= 0 && nx < grid.GetLength(0) && ny < grid.GetLength(1) &&
                        !grid[nx, ny] && !visited[nx, ny])
                    {
                        queue.Enqueue((nx, ny));
                        visited[nx, ny] = true;
                    }
                }
            }

            return false;
        }

        public int SolvePart1(string[] input)
        {
            const int gridSize = 71;
            var grid = new bool[gridSize, gridSize];

            for (int i = 0; i < Math.Min(input.Length, 1024); i++)
            {
                var coordinates = input[i].Split(',');
                int x = int.Parse(coordinates[0]);
                int y = int.Parse(coordinates[1]);
                grid[x, y] = true;
            }

            return FindShortestPath(grid, (0, 0), (gridSize - 1, gridSize - 1));
        }

        public int SolvePart2(string[] input)
        {
            const int gridSize = 71;
            var grid = new bool[gridSize, gridSize];

            for (int i = 0; i < input.Length; i++)
            {
                var coordinates = input[i].Split(',');
                int x = int.Parse(coordinates[0]);
                int y = int.Parse(coordinates[1]);
                grid[x, y] = true;

                if (!PathExists(grid, (0, 0), (gridSize - 1, gridSize - 1)))
                {
                    Console.WriteLine($"Part 2 (String): {x},{y}");
                    return 0;
                }
            }

            return -1;
        }

        public static void Solve()
        {
            string inputPath = "Days/Day18/input.txt";
            string[] lines = File.ReadAllLines(inputPath);

            var solution = new Solution();
            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(lines));
        }
    }
}

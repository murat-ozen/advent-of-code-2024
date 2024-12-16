using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day16
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

        private (int endX, int endY) FindEndTile(string[] map)
        {
            for (int y = 0; y < map.Length; y++)
                for (int x = 0; x < map[y].Length; x++)
                    if (map[y][x] == 'E')
                        return (x, y);
            throw new Exception("No end tile found");
        }

        private (int startX, int startY) FindStartTile(string[] map)
        {
            for (int y = 0; y < map.Length; y++)
                for (int x = 0; x < map[y].Length; x++)
                    if (map[y][x] == 'S')
                        return (x, y);
            throw new Exception("No start tile found");
        }

        private bool IsValidMove(string[] map, int x, int y)
        {
            return x >= 0 && x < map[0].Length && 
                   y >= 0 && y < map.Length && 
                   map[y][x] != '#';
        }

        public int SolvePart1(string[] input)
        {
            var (startX, startY) = FindStartTile(input);
            var (endX, endY) = FindEndTile(input);

            var priorityQueue = new PriorityQueue<(int score, int x, int y, int dir), int>();
            var dist = new Dictionary<(int x, int y, int dir), int>();
            var seen = new HashSet<(int x, int y, int dir)>();

            priorityQueue.Enqueue((0, startX, startY, 1), 0);

            while (priorityQueue.Count > 0)
            {
                priorityQueue.TryDequeue(out var state, out _);
                var (score, x, y, dir) = state;

                if (!dist.ContainsKey((x, y, dir)) || score < dist[(x, y, dir)])
                    dist[(x, y, dir)] = score;

                if (x == endX && y == endY)
                    return score;

                if (seen.Contains((x, y, dir)))
                    continue;

                seen.Add((x, y, dir));

                var (dx, dy) = Directions[dir];
                int newX = x + dx;
                int newY = y + dy;

                if (IsValidMove(input, newX, newY))
                    priorityQueue.Enqueue((score + 1, newX, newY, dir), score + 1);

                priorityQueue.Enqueue((score + 1000, x, y, (dir + 1) % 4), score + 1000);
                priorityQueue.Enqueue((score + 1000, x, y, (dir + 3) % 4), score + 1000);
            }

            return -1;
        }

        public int SolvePart2(string[] input)
        {
            var (startX, startY) = FindStartTile(input);
            var (endX, endY) = FindEndTile(input);

            var bestScore = SolvePart1(input);

            var distForward = new Dictionary<(int x, int y, int dir), int>();
            var seenForward = new HashSet<(int x, int y, int dir)>();
            var pqForward = new PriorityQueue<(int score, int x, int y, int dir), int>();
            pqForward.Enqueue((0, startX, startY, 1), 0);

            while (pqForward.Count > 0)
            {
                pqForward.TryDequeue(out var state, out _);
                var (score, x, y, dir) = state;

                if (!distForward.ContainsKey((x, y, dir)) || score < distForward[(x, y, dir)])
                    distForward[(x, y, dir)] = score;

                if (seenForward.Contains((x, y, dir)))
                    continue;

                seenForward.Add((x, y, dir));

                var (dx, dy) = Directions[dir];
                int newX = x + dx;
                int newY = y + dy;

                if (IsValidMove(input, newX, newY) && score + 1 <= bestScore)
                    pqForward.Enqueue((score + 1, newX, newY, dir), score + 1);

                pqForward.Enqueue((score + 1000, x, y, (dir + 1) % 4), score + 1000);
                pqForward.Enqueue((score + 1000, x, y, (dir + 3) % 4), score + 1000);
            }

            var distBackward = new Dictionary<(int x, int y, int dir), int>();
            var seenBackward = new HashSet<(int x, int y, int dir)>();
            var pqBackward = new PriorityQueue<(int score, int x, int y, int dir), int>();
            
            for (int dir = 0; dir < 4; dir++)
                pqBackward.Enqueue((0, endX, endY, dir), 0);

            while (pqBackward.Count > 0)
            {
                pqBackward.TryDequeue(out var state, out _);
                var (score, x, y, dir) = state;

                if (!distBackward.ContainsKey((x, y, dir)) || score < distBackward[(x, y, dir)])
                    distBackward[(x, y, dir)] = score;

                if (seenBackward.Contains((x, y, dir)))
                    continue;

                seenBackward.Add((x, y, dir));

                var (dx, dy) = Directions[(dir + 2) % 4];
                int newX = x + dx;
                int newY = y + dy;

                if (IsValidMove(input, newX, newY) && score + 1 <= bestScore)
                    pqBackward.Enqueue((score + 1, newX, newY, dir), score + 1);

                pqBackward.Enqueue((score + 1000, x, y, (dir + 1) % 4), score + 1000);
                pqBackward.Enqueue((score + 1000, x, y, (dir + 3) % 4), score + 1000);
            }

            var bestPathTiles = new HashSet<(int x, int y)>();
            for (int r = 0; r < input.Length; r++)
            {
                for (int c = 0; c < input[0].Length; c++)
                {
                    for (int dir = 0; dir < 4; dir++)
                    {
                        if (distForward.ContainsKey((c, r, dir)) && 
                            distBackward.ContainsKey((c, r, dir)) &&
                            distForward[(c, r, dir)] + distBackward[(c, r, dir)] == bestScore)
                        {
                            bestPathTiles.Add((c, r));
                        }
                    }
                }
            }

            return bestPathTiles.Count;
        }

        public static void Solve()
        {
            string inputPath = "Days/Day16/input.txt";
            string[] lines = File.ReadAllLines(inputPath);

            var solution = new Solution();
            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(lines));
        }
    }
}
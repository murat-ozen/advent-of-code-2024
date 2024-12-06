using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day6
{
    public class Solution : ISolver
    {
        private static readonly Dictionary<char, (int dx, int dy)> Directions = new()
        {
            { '^', (0, -1) },
            { '>', (1, 0) },
            { 'v', (0, 1) },
            { '<', (-1, 0) }
        };

        private static char TurnRight(char currentDirection)
        {
            return currentDirection switch
            {
                '^' => '>',
                '>' => 'v',
                'v' => '<',
                '<' => '^',
                _ => throw new ArgumentException("Invalid direction")
            };
        }

        private (int x, int y, char direction) guard;
        private (char[,], int, int) ParseMapAndFindGuard(string[] input)
        {
            int rows = input.Length;
            int cols = input[0].Length;

            char[,] map = new char[rows, cols];

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    map[y, x] = input[y][x];
                    if ("^>v<".Contains(map[y, x]))
                    {
                        guard = (x, y, map[y, x]);
                        map[y, x] = '.';
                    }
                }
            }
            
            return (map, rows, cols);
        }

        private (int visitedCount, bool isLooped) TraverseGuardPath(
            char[,] map, 
            (int x, int y, char direction) startGuard, 
            bool checkLoopOnly = false)
        {
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);

            var visited = new HashSet<(int x, int y)>();
            var fullPathVisited = new HashSet<(int x, int y, char direction)>();
            (int x, int y, char direction) current = startGuard;

            visited.Add((current.x, current.y));
            fullPathVisited.Add(current);

            while (true)
            {
                var (dx, dy) = Directions[current.direction];
                int nx = current.x + dx;
                int ny = current.y + dy;

                if (nx < 0 || ny < 0 || nx >= cols || ny >= rows || map[ny, nx] == '#')
                {
                    current.direction = TurnRight(current.direction);
                }
                else
                {
                    current = (nx, ny, current.direction);
                    
                    if (!checkLoopOnly)
                    {
                        visited.Add((current.x, current.y));
                    }
                }

                if (fullPathVisited.Contains(current))
                {
                    return (visited.Count, true);
                }
                fullPathVisited.Add(current);

                if (nx < 0 || ny < 0 || nx >= cols || ny >= rows)
                {
                    return (visited.Count, false);
                }
            }
        }

        public int SolvePart1(string[] input)
        {
            (char[,] map, int rows, int cols) = ParseMapAndFindGuard(input);
            var (visitedCount, _) = TraverseGuardPath(map, guard);
            return visitedCount;
        }

        public int SolvePart2(string[] input)
        {
            (char[,] map, int rows, int cols) = ParseMapAndFindGuard(input);

            int possibleObstructions = 0;
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    if (map[y, x] == '.' && (x != guard.x || y != guard.y))
                    {
                        map[y, x] = '#';
                        var (_, isLooped) = TraverseGuardPath(map, guard, checkLoopOnly: true);
                        if (isLooped)
                        {
                            possibleObstructions++;
                        }
                        map[y, x] = '.';
                    }
                }
            }

            return possibleObstructions;
        }


        public static void Solve()
        {
            string inputPath = "Days/Day6/input.txt";
            string[] lines = File.ReadAllLines(inputPath);
            
            var solution = new Solution();
            
            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(lines));
        }
    }
}
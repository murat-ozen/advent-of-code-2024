using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day12
{
    public class Solution : ISolver
    {
        private class Garden
        {
            private readonly string[] _grid;
            private readonly int _rows;
            private readonly int _cols;
            private static readonly (int dr, int dc)[] Directions = { (-1, 0), (0, 1), (1, 0), (0, -1) };

            public Garden(string[] input)
            {
                _grid = input;
                _rows = input.Length;
                _cols = input[0].Length;
            }

            public int CalculateTotalPrice(bool usePerimeter = true)
            {
                var seen = new HashSet<(int r, int c)>();
                long totalPrice = 0;

                for (int r = 0; r < _rows; r++)
                {
                    for (int c = 0; c < _cols; c++)
                    {
                        if (seen.Contains((r, c))) continue;
                        var region = FindRegion(r, c, seen);
                        totalPrice += usePerimeter 
                            ? region.Area * region.Perimeter
                            : region.Area * region.Sides;
                    }
                }

                return (int)totalPrice;
            }

            private (int Area, int Perimeter, int Sides) FindRegion(int startR, int startC, HashSet<(int r, int c)> seen)
            {
                var queue = new Queue<(int r, int c)>();
                queue.Enqueue((startR, startC));
                int area = 0;
                int perimeter = 0;
                var perimeterPoints = new Dictionary<(int dr, int dc), HashSet<(int r, int c)>>();

                while (queue.Count > 0)
                {
                    var (r, c) = queue.Dequeue();
                    if (seen.Contains((r, c))) continue;

                    seen.Add((r, c));
                    area++;

                    foreach (var (dr, dc) in Directions)
                    {
                        var (newR, newC) = (r + dr, c + dc);
                        if (IsValidPosition(newR, newC) && _grid[newR][newC] == _grid[r][c])
                        {
                            queue.Enqueue((newR, newC));
                        }
                        else
                        {
                            perimeter++;
                            if (!perimeterPoints.ContainsKey((dr, dc)))
                                perimeterPoints[(dr, dc)] = new HashSet<(int r, int c)>();
                            perimeterPoints[(dr, dc)].Add((r, c));
                        }
                    }
                }

                return (area, perimeter, CalculateSides(perimeterPoints));
            }

            private int CalculateSides(Dictionary<(int dr, int dc), HashSet<(int r, int c)>> perimeterPoints)
            {
                int sides = 0;
                foreach (var points in perimeterPoints.Values)
                {
                    var seenPerim = new HashSet<(int r, int c)>();
                    foreach (var point in points)
                    {
                        if (seenPerim.Contains(point)) continue;
                        sides++;
                        MarkConnectedPerimeterPoints(point, points, seenPerim);
                    }
                }
                return sides;
            }

            private void MarkConnectedPerimeterPoints((int r, int c) start, HashSet<(int r, int c)> points, HashSet<(int r, int c)> seen)
            {
                var queue = new Queue<(int r, int c)>();
                queue.Enqueue(start);

                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    if (seen.Contains(current)) continue;

                    seen.Add(current);
                    foreach (var (dr, dc) in Directions)
                    {
                        var next = (current.r + dr, current.c + dc);
                        if (points.Contains(next))
                        {
                            queue.Enqueue(next);
                        }
                    }
                }
            }

            private bool IsValidPosition(int r, int c) => 
                r >= 0 && r < _rows && c >= 0 && c < _cols;
        }

        public int SolvePart1(string[] input)
        {
            var garden = new Garden(input);
            return garden.CalculateTotalPrice(usePerimeter: true);
        }

        public int SolvePart2(string[] input)
        {
            var garden = new Garden(input);
            return garden.CalculateTotalPrice(usePerimeter: false);
        }

        public static void Solve()
        {
            string inputPath = "Days/Day12/input.txt";
            string[] lines = File.ReadAllLines(inputPath);

            var solution = new Solution();
            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(lines));
        }
    }
}
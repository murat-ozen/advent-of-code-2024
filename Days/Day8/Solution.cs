using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day8
{
    public class Solution : ISolver
    {
        private Dictionary<char, List<(int x, int y)>> ParseMap(string[] input)
        {
            var antennas = new Dictionary<char, List<(int x, int y)>>();

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    char cell = input[y][x];
                    if (cell != '.')
                    {
                        if (!antennas.ContainsKey(cell))
                            antennas[cell] = new List<(int x, int y)>();
                        antennas[cell].Add((x, y));
                    }
                }
            }
            return antennas;
        }

        public void AddAntinode(string[] input, HashSet<(int x, int y)> antinodePositions, List<(int x, int y)> positions, bool part2=false)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                for (int j = i + 1; j < positions.Count; j++)
                {
                    if(!part2)
                    {
                        var p1 = positions[i];
                        var p2 = positions[j];

                        var antinode1 = (x: p1.x - (p2.x - p1.x), y: p1.y - (p2.y - p1.y));
                        var antinode2 = (x: p2.x + (p2.x - p1.x), y: p2.y + (p2.y - p1.y));

                        if (IsWithinBounds(antinode1, input))
                            antinodePositions.Add(antinode1);
                        if (IsWithinBounds(antinode2, input))
                            antinodePositions.Add(antinode2);
                    }
                    else
                    {
                        if (i == j) continue;

                        var p1 = positions[i];
                        var p2 = positions[j];

                        AddAllPointsBetween(p1, p2, antinodePositions, input);
                    }
                        
                }
            }
            
        }

        static bool IsWithinBounds((int x, int y) position, string[] grid)
        {
            return position.y >= 0 && position.y < grid.Length &&
                position.x >= 0 && position.x < grid[0].Length;
        }

        static void AddAllPointsBetween((int x, int y) p1, (int x, int y) p2, HashSet<(int x, int y)> antinodePositions, string[] grid)
        {
            int dx = p2.x - p1.x;
            int dy = p2.y - p1.y;

            int gcd = GCD(Math.Abs(dx), Math.Abs(dy));
            dx /= gcd;
            dy /= gcd;

            var current = p1;
            while (IsWithinBounds(current, grid))
            {
                antinodePositions.Add(current);
                current = (current.x + dx, current.y + dy);
            }

            current = p1;
            while (IsWithinBounds(current, grid))
            {
                antinodePositions.Add(current);
                current = (current.x - dx, current.y - dy);
            }
        }

        static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public int SolvePart1(string[] input)
        {
            var antennas = ParseMap(input);
            
            var antinodePositions = new HashSet<(int x, int y)>();

            foreach (var entry in antennas)
            {
                var positions = entry.Value;

                for (int i = 0; i < positions.Count; i++)
                {
                    for (int j = i + 1; j < positions.Count; j++)
                    {
                        AddAntinode(input, antinodePositions, positions);
                    }
                }
            }

            return antinodePositions.Count;
        }

        public int SolvePart2(string[] input)
        {
            var antennas = ParseMap(input);
            
            var antinodePositions = new HashSet<(int x, int y)>();

            foreach (var entry in antennas)
            {
                var positions = entry.Value;

                for (int i = 0; i < positions.Count; i++)
                {
                    for (int j = 0; j < positions.Count; j++)
                    {
                        AddAntinode(input, antinodePositions, positions, part2: true);
                    }
                }
            }

            return antinodePositions.Count;
        }


        public static void Solve()
        {
            string inputPath = "Days/Day8/input.txt";
            string[] lines = File.ReadAllLines(inputPath);
            
            var solution = new Solution();
            
            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(lines));
        }
    }
}
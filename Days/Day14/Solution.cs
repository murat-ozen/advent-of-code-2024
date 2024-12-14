using System.Text.RegularExpressions;
using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day14
{
    public class Solution : ISolver
    {
        private List<(int x, int y, int vx, int vy)> ParseRobots(string[] input)
        {
            var robots = new List<(int x, int y, int vx, int vy)>();
            var regex = new Regex(@"p=(?<px>-?\d+),(?<py>-?\d+) v=(?<vx>-?\d+),(?<vy>-?\d+)");

            foreach (var line in input)
            {
                var match = regex.Match(line);
                if (match.Success)
                {
                    int px = int.Parse(match.Groups["px"].Value);
                    int py = int.Parse(match.Groups["py"].Value);
                    int vx = int.Parse(match.Groups["vx"].Value);
                    int vy = int.Parse(match.Groups["vy"].Value);
                    robots.Add((px, py, vx, vy));
                }
                else
                {
                    Console.WriteLine($"Invalid line format: {line}");
                }
            }

            return robots;
        }

        private int GetLargestComponent(List<(int x, int y, int vx, int vy)> robots, int seconds, int mapWidth, int mapHeight)
        {
            var visited = new HashSet<(int, int)>();
            var points = robots.Select(r => GetPosition(r, seconds, mapWidth, mapHeight)).ToHashSet();
            var largestComponent = 0;

            foreach (var point in points)
            {
                if (visited.Contains(point))
                    continue;

                var componentSize = GetComponentSize(point, points, visited);
                if (componentSize > largestComponent)
                    largestComponent = componentSize;
            }

            return largestComponent;
        }

        private int GetComponentSize((int x, int y) point, HashSet<(int x, int y)> points, HashSet<(int x, int y)> visited)
        {
            var queue = new Queue<(int x, int y)>();
            queue.Enqueue(point);
            visited.Add(point);
            var componentSize = 0;

            var directions = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                componentSize++;

                foreach (var (dx, dy) in directions)
                {
                    var next = (current.x + dx, current.y + dy);
                    if (visited.Contains(next) || !points.Contains(next))
                        continue;

                    visited.Add(next);
                    queue.Enqueue(next);
                }
            }

            return componentSize;
        }
        
        private (int x, int y) GetPosition((int x, int y, int vx, int vy) robot, int seconds, int mapWidth, int mapHeight)
        {
            var finalX = (robot.x + robot.vx * seconds) % mapWidth;
            var finalY = (robot.y + robot.vy * seconds) % mapHeight;

            if (finalX < 0)
                finalX += mapWidth;

            if (finalY < 0)
                finalY += mapHeight;

            return (finalX, finalY);
        }

        public int SolvePart1(string[] input)
        {
            const int width = 101;
            const int height = 103;

            var robots = ParseRobots(input);

            for (int t = 0; t < 100; t++)
            {
                for (int i = 0; i < robots.Count; i++)
                {
                    var (x, y, vx, vy) = robots[i];

                    x = (x + vx + width) % width;
                    y = (y + vy + height) % height;

                    robots[i] = (x, y, vx, vy);
                }
            }

            int midX = width / 2;
            int midY = height / 2;
            int q1 = 0, q2 = 0, q3 = 0, q4 = 0;

            foreach (var (x, y, _, _) in robots)
            {
                if (x == midX || y == midY)
                    continue;

                if (x > midX && y < midY) q1++;
                else if (x < midX && y < midY) q2++;
                else if (x < midX && y > midY) q3++;
                else if (x > midX && y > midY) q4++;
            }

            int safetyFactor = q1 * q2 * q3 * q4;
            
            return safetyFactor;
        }

        public int SolvePart2(string[] input)
        {
            var robots = ParseRobots(input);
            const int mapWidth = 101;
            const int mapHeight = 103;

            int maxComponentSeconds = 0;
            int maxComponent = 0;

            for (int seconds = 0; seconds < 10000; seconds++)
            {
                var largestComponent = GetLargestComponent(robots, seconds, mapWidth, mapHeight);
                if (largestComponent > maxComponent)
                {
                    maxComponentSeconds = seconds;
                    maxComponent = largestComponent;
                }
            }

            return maxComponentSeconds;
        }

        public static void Solve()
        {
            string inputPath = "Days/Day14/input.txt";
            string[] lines = File.ReadAllLines(inputPath);

            var solution = new Solution();
            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(lines));
        }
    }
}
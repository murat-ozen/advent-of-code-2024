using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day21
{
    public class Solution : ISolver
    {
        private readonly string[] pad1 = { "789", "456", "123", " 0A" };
        private readonly string[] pad2 = { " ^A", "<v>" };
        private readonly Dictionary<string, (int, int)> startPositions = new()
        {
            {"^", (0, 1)}, {"<", (1, 0)}, {"v", (1, 1)}, {">", (1, 2)}, {"A", (0, 2)}
        };

        private char? GetPad1((int r, int c) p)
        {
            if (p.r < 0 || p.r >= pad1.Length || p.c < 0 || p.c >= pad1[p.r].Length)
                return null;
            char value = pad1[p.r][p.c];
            return value == ' ' ? null : value;
        }

        private char? GetPad2((int r, int c) p)
        {
            if (p.r < 0 || p.r >= pad2.Length || p.c < 0 || p.c >= pad2[p.r].Length)
                return null;
            char value = pad2[p.r][p.c];
            return value == ' ' ? null : value;
        }

        private ((int r, int c), char?) ApplyMove((int r, int c) p, string move, Func<(int r, int c), char?> padGetter)
        {
            return move switch
            {
                "A" => (p, padGetter(p)),
                "<" => ((p.r, p.c - 1), null),
                "^" => ((p.r - 1, p.c), null),
                ">" => ((p.r, p.c + 1), null),
                "v" => ((p.r + 1, p.c), null),
                _ => throw new ArgumentException("Invalid move")
            };
        }

        private readonly Dictionary<(string ch, string prevMove, int pads), long> dp = new();

        private long Cost2(string ch, string prevMove, int pads)
        {
            var key = (ch, prevMove, pads);
            if (dp.ContainsKey(key))
                return dp[key];

            if (pads == 0)
                return 1;

            var queue = new PriorityQueue<(long d, (int r, int c) p, string prev, string output, string path), long>();
            var seen = new Dictionary<((int r, int c) p, string prev), long>();
            var startPos = startPositions[prevMove];
            
            queue.Enqueue((0, startPos, "A", "", ""), 0);

            while (queue.Count > 0)
            {
                var (d, p, prev, output, path) = queue.Dequeue();

                if (GetPad2(p) == null)
                    continue;

                if (output == ch)
                {
                    dp[key] = d;
                    return d;
                }
                else if (output.Length > 0)
                    continue;

                var seenKey = (p, prev);
                if (seen.TryGetValue(seenKey, out long seenDist))
                {
                    if (d >= seenDist)
                        continue;
                }
                seen[seenKey] = d;

                foreach (string move in new[] { "^", "<", "v", ">", "A" })
                {
                    var (newP, moveOutput) = ApplyMove(p, move, GetPad2);
                    long costMove = Cost2(move, prev, pads - 1);
                    long newD = d + costMove;
                    string newOut = output;
                    if (moveOutput.HasValue)
                        newOut += moveOutput.Value;
                    
                    queue.Enqueue((newD, newP, move, newOut, path), newD);
                }
            }

            throw new Exception($"No solution found for {ch}, {pads}");
        }

        private long Solve1(string code, int pads)
        {
            var queue = new PriorityQueue<(long d, (int r, int c) p1, string p2, string output, string path), long>();
            var seen = new Dictionary<((int r, int c) p1, string p2, string output), long>();
            
            queue.Enqueue((0, (3, 2), "A", "", ""), 0);

            while (queue.Count > 0)
            {
                var (d, p1, p2, output, path) = queue.Dequeue();

                if (output == code)
                    return d;

                if (!code.StartsWith(output))
                    continue;

                if (GetPad1(p1) == null)
                    continue;

                var key = (p1, p2, output);
                if (seen.TryGetValue(key, out long seenDist))
                {
                    if (d >= seenDist)
                        continue;
                }
                seen[key] = d;

                foreach (string move in new[] { "^", "<", "v", ">", "A" })
                {
                    var (newP1, moveOutput) = ApplyMove(p1, move, GetPad1);
                    long costMove = Cost2(move, p2, pads);
                    string newOut = output;
                    if (moveOutput.HasValue)
                        newOut += moveOutput.Value;

                    queue.Enqueue((d + costMove, newP1, move, newOut, path), d + costMove);
                }
            }

            throw new Exception($"No solution found for {code}");
        }

        public int SolvePart1(string[] input)
        {
            long result = 0;
            foreach (string code in input)
            {
                int value = int.Parse(code.TrimEnd('A'));
                long length = Solve1(code, 2);
                result += value * length;
            }
            return (int)result;
        }

        public int SolvePart2(string[] input)
        {
            long result = 0;
            foreach (string code in input)
            {
                long value = long.Parse(code.TrimEnd('A'));
                long length = Solve1(code, 25);
                result += value * length;
            }
            Console.WriteLine($"Part 2 (Long): {result}");
            return 0;
        }

        public static void Solve()
        {
            string inputPath = "Days/Day21/input.txt";
            string[] lines = File.ReadAllLines(inputPath);

            var solution = new Solution();
            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(lines));
        }
    }
}
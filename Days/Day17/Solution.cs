using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day17
{
    public class Solution : ISolver
    {
        private List<long> Operands = new();

        private (long, long, long, List<long>) ParseInput(string[] input)
        {
            long A = long.Parse(input[0].Split(":")[1].Trim());
            long B = long.Parse(input[1].Split(":")[1].Trim());
            long C = long.Parse(input[2].Split(":")[1].Trim());

            var operands = input[4].Split(":")[1].Trim()
                                  .Split(',')
                                  .Select(long.Parse)
                                  .ToList();

            return (A, B, C, operands);
        }

        public int SolvePart1(string[] input)
        {
            var (A, B, C, operands) = ParseInput(input);
            Operands = operands;

            var output = RunProgram(A);
            Console.WriteLine("Part 1 (String): " + string.Join(",", output));
            return 0;
        }

        public int SolvePart2(string[] input)
        {
            var (A, B, C, operands) = ParseInput(input);
            Operands = operands;
            Console.WriteLine("Part 2 (Long): " + cursedDFS(0, 0).Min());
            return 0;
        }

        private List<long> cursedDFS(long curVal, int depth)
        {
            List<long> res = new();
            if (depth > Operands.Count) return res;
            var tmp = curVal << 3;
            for (int i = 0; i < 8; i++)
            {
                var tmpRes = RunProgram(tmp + i);
                if (tmpRes.SequenceEqual(Operands.TakeLast(depth + 1)))
                {
                    if (depth + 1 == Operands.Count) res.Add(tmp + i);
                    res.AddRange(cursedDFS(tmp + i, depth + 1));
                }
            }

            return res;
        }

        private List<long> RunProgram(long A)
        {
            long B = 0;
            long C = 0;
            List<long> output = new();
            int pc = 0;

            while (pc < Operands.Count)
            {
                long combo = Operands[pc + 1] switch
                {
                    0 => 0,
                    1 => 1,
                    2 => 2,
                    3 => 3,
                    4 => A,
                    5 => B,
                    6 => C,
                    _ => long.MinValue
                };

                long literal = Operands[pc + 1];
                long res = 0;
                bool jumped = false;

                switch (Operands[pc])
                {
                    case 0:
                        res = (long)(A / Math.Pow(2, combo));
                        A = res;
                        break;
                    case 1:
                        res = B ^ literal;
                        B = res;
                        break;
                    case 2:
                        res = combo % 8;
                        B = res;
                        break;
                    case 3:
                        if (A != 0)
                        {
                            pc = (int)literal;
                            jumped = true;
                        }
                        break;
                    case 4:
                        res = B ^ C;
                        B = res;
                        break;
                    case 5:
                        output.Add(combo % 8);
                        break;
                    case 6:
                        res = (long)(A / Math.Pow(2, combo));
                        B = res;
                        break;
                    case 7:
                        res = (long)(A / Math.Pow(2, combo));
                        C = res;
                        break;
                    default:
                        break;
                }

                if (!jumped) pc += 2;
                if (output.Count > Operands.Count) break;
            }

            return output;
        }

        public static void Solve()
        {
            string inputPath = "Days/Day17/input.txt";
            string[] lines = File.ReadAllLines(inputPath);

            var solution = new Solution();
            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(lines));
        }
    }
}

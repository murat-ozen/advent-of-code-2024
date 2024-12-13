using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day13
{
    public class Solution : ISolver
    {
        private int Solve(string[] input, bool isPart2)
        {
            long total = 0;
            var inputReader = new StringReader(string.Join(Environment.NewLine, input));
            while (TryReadClawMachine(inputReader, out var clawMachine))
            {
                var price = clawMachine.CalculatePrice(isPart2);
                if (price > 0)
                {
                    total += price;
                }
            }
            if (isPart2)
                Console.WriteLine($"Part 2 (Long): {total}");
            return (int)Math.Min(total, int.MaxValue);
        }

        private bool TryReadClawMachine(StringReader reader, out ClawMachine clawMachine)
        {
            clawMachine = new ClawMachine(Tuple.Create(0, 0), Tuple.Create(0, 0), Tuple.Create(0, 0));
            var line = reader.ReadLine();
            if (line == null)
                return false;

            if (string.IsNullOrWhiteSpace(line))
                line = reader.ReadLine();

            var buttonALine = line;
            var buttonBLine = reader.ReadLine();
            var prizeLine = reader.ReadLine();

            if (buttonALine == null || buttonBLine == null || prizeLine == null)
            {
                return false;
            }

            clawMachine = ClawMachine.Parse(buttonALine, buttonBLine, prizeLine);
            return true;
        }

        public record ClawMachine(Tuple<int, int> ButtonA, Tuple<int, int> ButtonB, Tuple<int, int> Prize)
        {
            public static ClawMachine Parse(string buttonALine, string buttonBLine, string prizeLine)
            {
                var buttonA = ParsePoint(buttonALine);
                var buttonB = ParsePoint(buttonBLine);
                var prize = ParsePoint(prizeLine);

                return new ClawMachine(buttonA, buttonB, prize);
            }

            private static Tuple<int, int> ParsePoint(string line)
            {
                var parts = line.Split(":")[1].Split(',');
                var x = int.Parse(parts[0].Trim().Substring(2));
                var y = int.Parse(parts[1].Trim().Substring(2));
                return Tuple.Create(x, y);
            }

            public long CalculatePrice(bool isPart2 = false)
            {
                var prizeX = isPart2 ? 10000000000000 + Prize.Item1 : (double)Prize.Item1;
                var prizeY = isPart2 ? 10000000000000 + Prize.Item2 : (double)Prize.Item2;
                var buttonAX = (double)ButtonA.Item1;
                var buttonAY = (double)ButtonA.Item2;
                var buttonBX = (double)ButtonB.Item1;
                var buttonBY = (double)ButtonB.Item2;

                long b = (long)Math.Round((prizeY - (prizeX / buttonAX) * buttonAY) / (buttonBY - (buttonBX / buttonAX) * buttonAY));
                long a = (long)Math.Round((prizeX - b * buttonBX) / buttonAX);

                var actualX = a * buttonAX + b * buttonBX;
                var actualY = a * buttonAY + b * buttonBY;

                if (actualX == prizeX && actualY == prizeY && a >= 0 && b >= 0)
                {
                    return a * 3 + b;
                }

                return -1;
            }
        }

        public int SolvePart1(string[] input)
        {
            return Solve(input, false);
        }

        public int SolvePart2(string[] input)
        {
            return Solve(input, true);
        }

        public static void Solve()
        {
            string inputPath = "Days/Day13/input.txt";
            string[] lines = File.ReadAllLines(inputPath);

            var solution = new Solution();
            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2 (Max Integer): " + solution.SolvePart2(lines));
        }
    }
}
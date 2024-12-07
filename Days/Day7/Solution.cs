using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day7
{
    public class Solution : ISolver
    {
        static IEnumerable<char[]> GetOperatorCombinations(char[] operators, int length)
        {
            if (length == 0)
            {
                yield return new char[0];
                yield break;
            }

            foreach (var op in operators)
            {
                foreach (var rest in GetOperatorCombinations(operators, length - 1))
                {
                    yield return new[] {op}.Concat(rest).ToArray();
                }
            }
        }

        static long EvaluateExpression(int[] numbers, char[] operators)
        {
            long result = numbers[0];

            for (int i = 0; i < operators.Length; i++)
            {
                if (operators[i] == '+')
                {
                    result += numbers[i + 1];
                }
                else if (operators[i] == '*')
                {
                    result *= numbers[i + 1];
                }
                else if (operators[i] == '|')
                {
                    string left = result.ToString();
                    string right = numbers[i + 1].ToString();
                    result = long.Parse(left + right);
                }
            }
            return result;
        }

        static bool CanProduceTargetPart1(int[] numbers, long target)
        {
            int numOperators = numbers.Length - 1;
            var operators = new[] {'+', '*'};
            var combinations = GetOperatorCombinations(operators, numOperators);

            foreach (var combo in combinations)
            {
                if (EvaluateExpression(numbers, combo) == target)
                {
                    return true;
                }
            }
            return false;
        }
        
        static bool CanProduceTargetPart2(int[] numbers, long target)
        {
            int numOperators = numbers.Length - 1;
            var operators = new[] {'+', '*', '|'};
            var combinations = GetOperatorCombinations(operators, numOperators);

            foreach (var combo in combinations)
            {
                if (EvaluateExpression(numbers, combo) == target)
                {
                    return true;
                }
            }
            return false;
        }

        public int SolvePart1(string[] input)
        {
            long totalResult = 0;
            foreach (var line in input)
            {
                var parts = line.Split(":");
                long target = long.Parse(parts[0].Trim());
                var numbers = parts[1].Trim().Split(" ").Select(int.Parse).ToArray();
                
                if (CanProduceTargetPart1(numbers, target))
                {
                    totalResult += target;
                }
            }
            Console.WriteLine($"Part 1 (Long): {totalResult}");
            return (int)Math.Min(totalResult, int.MaxValue);
        }

        public int SolvePart2(string[] input)
        {
            long totalResult = 0;
            foreach (var line in input)
            {
                var parts = line.Split(":");
                long target = long.Parse(parts[0].Trim());
                var numbers = parts[1].Trim().Split(" ").Select(int.Parse).ToArray();
                
                if (CanProduceTargetPart2(numbers, target))
                {
                    totalResult += target;
                }
            }
            Console.WriteLine($"Part 2 (Long): {totalResult}");
            return (int)Math.Min(totalResult, int.MaxValue);
        }


        public static void Solve()
        {
            string inputPath = "Days/Day7/input.txt";
            string[] lines = File.ReadAllLines(inputPath);
            
            var solution = new Solution();
            
            Console.WriteLine("Part 1 (Max Integer): " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2 (Max Integer): " + solution.SolvePart2(lines));
        }
    }
}
using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day2
{
    public class Solution : ISolver
    {
        private bool IsSafe(List<int> inputs)
        {
            bool isIncreasing = true;
            bool isDecreasing = true;
                
            for(int i = 0; i < inputs.Count - 1; i++)
            {
                int diff = inputs[i + 1] - inputs[i];

                if(Math.Abs(diff) > 3 || Math.Abs(diff) < 1)
                {
                    isDecreasing = false;
                    isIncreasing = false;
                    break;
                }
                    
                if (diff < 0) isIncreasing = false;
                if (diff > 0) isDecreasing = false;
            }

            return isDecreasing || isIncreasing;
        }

        public int SolvePart1(string[] input)
        {
            int safeReports = 0;
            
            foreach (var line in input)
            {
                var inputs = line.Split(' ').Select(int.Parse).ToList();

                if (IsSafe(inputs))
                {
                    safeReports++;
                }

                
            }
            return safeReports;
        }

        public int SolvePart2(string[] input)
        {
            int safeReports = 0;
            
            foreach (var line in input)
            {
                var inputs = line.Split(' ').Select(int.Parse).ToList();

                if(IsSafe(inputs))
                {
                    safeReports++;
                }
                else
                {
                    for (int i = 0; i < inputs.Count; i++)
                    {
                        var modifiedInputs = new List<int>(inputs);
                        modifiedInputs.RemoveAt(i);

                        if (IsSafe(modifiedInputs))
                        {
                            safeReports++;
                            break;
                        }
                    }
                }
            }
            return safeReports;
        }

        public static void Solve()
        {
            string inputPath = "Days/Day2/input.txt";
            string[] lines = File.ReadAllLines(inputPath);
            
            var solution = new Solution();
            
            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(lines));
        }
    }
}
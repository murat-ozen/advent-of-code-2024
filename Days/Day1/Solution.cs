using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day1
{
    public class Solution : ISolver
    {
        public int SolvePart1(string[] input)
        {
            var leftList = new List<int>();
            var rightList = new List<int>();

            foreach (var line in input)
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                leftList.Add(int.Parse(parts[0]));
                rightList.Add(int.Parse(parts[1]));
            }

            leftList.Sort();
            rightList.Sort();

            int totalDistance = 0;

            for (int i = 0; i < leftList.Count; i++)
            {
                totalDistance += Math.Abs(leftList[i] - rightList[i]);
            }

            return totalDistance;
        }

        public int SolvePart2(string[] input)
        {
            var leftList = new List<int>();
            var rightList = new List<int>();

            foreach (var line in input)
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                leftList.Add(int.Parse(parts[0]));
                rightList.Add(int.Parse(parts[1]));
            }

            leftList.Sort();
            rightList.Sort();

            int similarityScore = 0;

            foreach (int i in leftList)
            {
                int count = rightList.Count(c => c == i);
                int similarity = i * count;
                similarityScore += similarity;
            }

            return similarityScore;
        }

        public static void Solve()
        {
            string inputPath = "Days/Day1/input.txt";
            string[] lines = File.ReadAllLines(inputPath);
            
            var solution = new Solution();
            
            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(lines));
        }
    }
}
using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day5
{
    public class Solution : ISolver
    {
        private Dictionary<int, HashSet<int>> ParseRules(string[] input, out int ruleEndIndex)
        {
            var rules = new Dictionary<int, HashSet<int>>();
            ruleEndIndex = 0;

            while (ruleEndIndex < input.Length && !string.IsNullOrEmpty(input[ruleEndIndex]))
            {
                var parts = input[ruleEndIndex].Split('|');
                var before = int.Parse(parts[0]);
                var after = int.Parse(parts[1]);

                if (!rules.ContainsKey(before))
                    rules[before] = new HashSet<int>();
                rules[before].Add(after);

                ruleEndIndex++;
            }

            return rules;
        }

        private List<List<int>> ParseUpdates(string[] input, int startIndex)
        {
            var updates = new List<List<int>>();
            
            for (int i = startIndex + 1; i < input.Length; i++)
            {
                if (!string.IsNullOrEmpty(input[i]))
                {
                    updates.Add(input[i].Split(',')
                        .Select(int.Parse)
                        .ToList());
                }
            }

            return updates;
        }

        private bool IsValidOrder(List<int> pages, Dictionary<int, HashSet<int>> rules)
        {
            var pagePositions = new Dictionary<int, int>();
            for (int i = 0; i < pages.Count; i++)
            {
                pagePositions[pages[i]] = i;
            }

            for (int i = 0; i < pages.Count; i++)
            {
                var page = pages[i];
                if (rules.ContainsKey(page))
                {
                    foreach (var mustComeAfter in rules[page])
                    {
                        if (pagePositions.ContainsKey(mustComeAfter) && 
                            pagePositions[mustComeAfter] <= i)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public int SolvePart1(string[] input)
        {
            var rules = ParseRules(input, out int ruleEndIndex);
            var updates = ParseUpdates(input, ruleEndIndex);

            int total = 0;
            foreach (var update in updates)
            {
                if (IsValidOrder(update, rules))
                {
                    int middleIdx = update.Count / 2;
                    total += update[middleIdx];
                }
            }
            return total;
        }

        private List<int> OrderPages(List<int> pages, Dictionary<int, HashSet<int>> rules)
        {
            var graph = new Dictionary<int, HashSet<int>>();
            var inDegree = new Dictionary<int, int>();

            foreach (var page in pages)
            {
                graph[page] = new HashSet<int>();
                inDegree[page] = 0;
            }

            foreach (var page in pages)
            {
                if (rules.ContainsKey(page))
                {
                    foreach (var after in rules[page])
                    {
                        if (pages.Contains(after))
                        {
                            graph[page].Add(after);
                            inDegree[after] = inDegree.GetValueOrDefault(after, 0) + 1;
                        }
                    }
                }
            }

            var queue = new Queue<int>();
            foreach (var page in pages)
            {
                if (inDegree[page] == 0)
                {
                    queue.Enqueue(page);
                }
            }

            var result = new List<int>();

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                result.Add(current);

                foreach (var neighbor in graph[current])
                {
                    inDegree[neighbor]--;
                    if (inDegree[neighbor] == 0)
                    {
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return result;
        }

        public int SolvePart2(string[] input)
        {
            var rules = ParseRules(input, out int ruleEndIndex);
            var updates = ParseUpdates(input, ruleEndIndex);

            int total = 0;
            foreach (var update in updates)
            {
                if (!IsValidOrder(update, rules))
                {
                    var orderedUpdate = OrderPages(update.ToList(), rules);
                    int middleIdx = orderedUpdate.Count / 2;
                    total += orderedUpdate[middleIdx];
                }
            }
            return total;
        }

        public static void Solve()
        {
            string inputPath = "Days/Day5/input.txt";
            string[] lines = File.ReadAllLines(inputPath);
            
            var solution = new Solution();
            
            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(lines));
        }
    }
}
using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day23
{
    public class Solution : ISolver
    {
        private void FindCliques(
            Dictionary<string, HashSet<string>> graph,
            HashSet<string> r,
            HashSet<string> p,
            HashSet<string> x,
            Action<HashSet<string>> onCliqueFound)
        {
            if (!p.Any() && !x.Any())
            {
                onCliqueFound(new HashSet<string>(r));
                return;
            }

            var pivot = p.Concat(x).First();
            foreach (var v in p.Except(graph[pivot]))
            {
                r.Add(v);
                FindCliques(graph, r, new HashSet<string>(p.Intersect(graph[v])), new HashSet<string>(x.Intersect(graph[v])), onCliqueFound);
                r.Remove(v);
                p.Remove(v);
                x.Add(v);
            }
        }
          
        public int SolvePart1(string[] input)
        {
            var graph = new Dictionary<string, HashSet<string>>();
            foreach (var line in input)
            {
                var parts = line.Split('-');
                if (!graph.ContainsKey(parts[0]))
                    graph[parts[0]] = new HashSet<string>();
                if (!graph.ContainsKey(parts[1]))
                    graph[parts[1]] = new HashSet<string>();

                graph[parts[0]].Add(parts[1]);
                graph[parts[1]].Add(parts[0]);
            }

            var triads = new HashSet<string>();
            foreach (var node in graph.Keys)
            {
                foreach (var neighbor1 in graph[node])
                {
                    foreach (var neighbor2 in graph[neighbor1])
                    {
                        if (neighbor2 != node && graph[node].Contains(neighbor2))
                        {
                            var triad = new List<string> { node, neighbor1, neighbor2 };
                            triad.Sort();
                            triads.Add(string.Join(",", triad));
                        }
                    }
                }
            }

            var filteredTriads = triads.Where(triad => triad.Split(',').Any(name => name.StartsWith("t"))).ToList();

            return filteredTriads.Count;
        }

        public int SolvePart2(string[] input)
        {
            var graph = new Dictionary<string, HashSet<string>>();
            foreach (var line in input)
            {
                var parts = line.Split('-');
                if (!graph.ContainsKey(parts[0]))
                    graph[parts[0]] = new HashSet<string>();
                if (!graph.ContainsKey(parts[1]))
                    graph[parts[1]] = new HashSet<string>();

                graph[parts[0]].Add(parts[1]);
                graph[parts[1]].Add(parts[0]);
            }

            var maxClique = new List<string>();
            FindCliques(graph, new HashSet<string>(), new HashSet<string>(graph.Keys), new HashSet<string>(), clique =>
            {
                if (clique.Count > maxClique.Count)
                {
                    maxClique = clique.ToList();
                }
            });

            maxClique.Sort();
            
            Console.WriteLine($"Part 2 (String): {string.Join(",", maxClique)}");
            return 0;
        }

        public static void Solve()
        {
            string inputPath = "Days/Day23/input.txt";
            string[] lines = File.ReadAllLines(inputPath);

            var solution = new Solution();
            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(lines));
        }
    }
}

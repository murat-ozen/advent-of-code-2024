using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day15
{
    public class Solution : ISolver
    {
        private int Simulate(string[] input, bool isPart2)
        {
            string map = string.Join("\n", input.TakeWhile(line => !string.IsNullOrEmpty(line)));
            string instructions = string.Join("", input.SkipWhile(line => !string.IsNullOrEmpty(line)).Skip(1));

            var mapLines = map.Split('\n');
            int numRows = mapLines.Length;
            int numCols = mapLines[0].Length;

            var grid = mapLines.Select(line => line.ToCharArray()).ToArray();

            if (isPart2)
            {
                var expandedGrid = new List<char[]>();
                for (int row = 0; row < numRows; row++)
                {
                    var newRow = new List<char>();
                    for (int col = 0; col < numCols; col++)
                    {
                        if (grid[row][col] == '#')
                        {
                            newRow.Add('#');
                            newRow.Add('#');
                        }
                        else if (grid[row][col] == 'O')
                        {
                            newRow.Add('[');
                            newRow.Add(']');
                        }
                        else if (grid[row][col] == '.')
                        {
                            newRow.Add('.');
                            newRow.Add('.');
                        }
                        else if (grid[row][col] == '@')
                        {
                            newRow.Add('@');
                            newRow.Add('.');
                        }
                    }
                    expandedGrid.Add(newRow.ToArray());
                }
                grid = expandedGrid.ToArray();
                numCols *= 2;
            }

            int startRow = -1, startCol = -1;
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    if (grid[row][col] == '@')
                    {
                        startRow = row;
                        startCol = col;
                        grid[row][col] = '.';
                        break;
                    }
                }
                if (startRow != -1) break;
            }

            int currentRow = startRow;
            int currentCol = startCol;

            foreach (char instruction in instructions)
            {
                var (rowChange, colChange) = instruction switch
                {
                    '^' => (-1, 0),
                    '>' => (0, 1),
                    'v' => (1, 0),
                    '<' => (0, -1),
                    _ => (0, 0)
                };

                int nextRow = currentRow + rowChange;
                int nextCol = currentCol + colChange;

                if (grid[nextRow][nextCol] == '#')
                    continue;
                else if (grid[nextRow][nextCol] == '.')
                {
                    currentRow = nextRow;
                    currentCol = nextCol;
                }
                else if (grid[nextRow][nextCol] == 'O' || grid[nextRow][nextCol] == '[' || grid[nextRow][nextCol] == ']')
                {
                    var queue = new Queue<(int, int)>();
                    queue.Enqueue((currentRow, currentCol));
                    var visited = new HashSet<(int, int)>();

                    bool isPathClear = true;
                    while (queue.Count > 0)
                    {
                        var (currentR, currentC) = queue.Dequeue();
                        if (visited.Contains((currentR, currentC)))
                            continue;

                        visited.Add((currentR, currentC));

                        int nextRowTemp = currentR + rowChange;
                        int nextColTemp = currentC + colChange;

                        if (grid[nextRowTemp][nextColTemp] == '#')
                        {
                            isPathClear = false;
                            break;
                        }

                        if (grid[nextRowTemp][nextColTemp] == 'O')
                            queue.Enqueue((nextRowTemp, nextColTemp));

                        if (grid[nextRowTemp][nextColTemp] == '[')
                        {
                            queue.Enqueue((nextRowTemp, nextColTemp));
                            queue.Enqueue((nextRowTemp, nextColTemp + 1));
                        }

                        if (grid[nextRowTemp][nextColTemp] == ']')
                        {
                            queue.Enqueue((nextRowTemp, nextColTemp));
                            queue.Enqueue((nextRowTemp, nextColTemp - 1));
                        }
                    }

                    if (!isPathClear)
                        continue;

                    while (visited.Count > 0)
                    {
                        var sortedVisited = visited.OrderBy(v => v.Item1).ThenBy(v => v.Item2).ToList();
                        foreach (var (visitedRow, visitedCol) in sortedVisited)
                        {
                            int nextRowTemp = visitedRow + rowChange;
                            int nextColTemp = visitedCol + colChange;

                            if (!visited.Contains((nextRowTemp, nextColTemp)))
                            {
                                grid[nextRowTemp][nextColTemp] = grid[visitedRow][visitedCol];
                                grid[visitedRow][visitedCol] = '.';
                                visited.Remove((visitedRow, visitedCol));
                            }
                        }
                    }

                    currentRow += rowChange;
                    currentCol += colChange;
                }
            }

            int result = 0;
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    if (grid[row][col] == '[' || grid[row][col] == 'O')
                    {
                        result += 100 * row + col;
                    }
                }
            }

            return result;
        }

        public int SolvePart1(string[] input)
        {
            return Simulate(input, false);
        }

        public int SolvePart2(string[] input)
        {
            return Simulate(input, true);
        }

        public static void Solve()
        {
            string inputPath = "Days/Day15/input.txt";
            string[] inputLines = File.ReadAllLines(inputPath);

            var solution = new Solution();
            Console.WriteLine("Part 1: " + solution.SolvePart1(inputLines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(inputLines));
        }
    }
}
using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day4
{
    public class Solution : ISolver
    {
        private char[,] CreateGrid(string[] lines)
        {
            int rows = lines.Length;
            int cols = lines[0].Length;
            char[,] grid = new char[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    grid[i, j] = lines[i][j];
                }
            }
            return grid;
        }
        private int CountOccurrences(char[,] grid)
        {
            string target = "XMAS";
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            int totalCount = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (grid[i, j] != 'X') continue;
                    totalCount += CountDirectionalOccurrences(grid, i, j, target);
                }
            }
            return totalCount;
        }

        private int CountDirectionalOccurrences(char[,] grid, int i, int j, string target)
        {
            int count = 0;
            int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int dir = 0; dir < 8; dir++)
            {
                if (IsWordPresent(grid, i, j, dx[dir], dy[dir], target))
                {
                    count++;
                }
            }
            return count;
        }

        private bool IsWordPresent(char[,] grid, int startI, int startJ, int dx, int dy, string target)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            for (int k = 0; k < target.Length; k++)
            {
                int newRow = startI + k * dx;
                int newCol = startJ + k * dy;

                if (!IsValid(newRow, newCol, rows, cols) || 
                    grid[newRow, newCol] != target[k])
                {
                    return false;
                }
            }
            return true;
        }

        public int SolvePart1(string[] input)
        {
            var grid = CreateGrid(input);
            int count = CountOccurrences(grid);
            return count;
        }

        private int CountXMasPatterns(char[,] grid)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            int count = 0;
            for (int i = 1; i < rows - 1; i++)
            {
                for (int j = 1; j < cols - 1; j++)
                {
                    if (grid[i, j] != 'A') continue;
                    count += CheckXMAS(i, j, grid, rows, cols);
                }
            }
            return count;
        }

        private int CheckXMAS(int centerI, int centerJ, char[,] grid, int rows, int cols)
        {
            int count = 0;
            string[] patterns = { "MAS", "SAM" };
            
            foreach (string pattern1 in patterns)
            {
                foreach (string pattern2 in patterns)
                {
                    if (CheckDiagonal(centerI, centerJ, pattern1, -1, -1, grid, rows, cols) && CheckDiagonal(centerI, centerJ, pattern2, -1, 1, grid, rows, cols))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private bool CheckDiagonal(int centerI, int centerJ, string pattern, int di, int dj, char[,] grid, int rows, int cols)
        {
            if (!IsValid(centerI + di, centerJ + dj, rows, cols) || grid[centerI + di, centerJ + dj] != pattern[0])
            {
                return false;
            }
                
            if (!IsValid(centerI - di, centerJ - dj, rows, cols) || grid[centerI - di, centerJ - dj] != pattern[2])
            {
                return false;
            }
            return true;
        }

        private bool IsValid(int row, int col, int rows, int cols)
        {
            return row >= 0 && row < rows && col >= 0 && col < cols;
        }

        public int SolvePart2(string[] input)
        {
            var grid = CreateGrid(input);
            int count = CountXMasPatterns(grid);
            return count;
        }

        public static void Solve()
        {
            string inputPath = "Days/Day4/input.txt";
            string[] lines = File.ReadAllLines(inputPath);
            
            var solution = new Solution();
            
            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(lines));
        }
    }
}
using System.Text;
using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day9
{
    public class Solution : ISolver
    {
        public class FileBlock
        {
            public int Id { get; }
            public int Start { get; set; }
            public int Length { get; }

            public FileBlock(int id, int start, int length)
            {
                Id = id;
                Start = start;
                Length = length;
            }
        }

        public class FreeBlock
        {
            public int Start { get; set;}
            public int Length { get; set; }

            public FreeBlock(int start, int length)
            {
                Start = start;
                Length = length;
            }
        }

        public class DiskMap
        {
            public List<FileBlock> Files { get; private set; }
            public Queue<FreeBlock> FreeSpace { get; private set; }
            public StringBuilder Output { get; private set; }

            public DiskMap(string diskMap)
            {
                Files = new List<FileBlock>();
                FreeSpace = new Queue<FreeBlock>();
                Output = new StringBuilder();
                ProcessDiskMap(diskMap);
            }

            private void ProcessDiskMap(string diskMap)
            {
                bool isFile = true;
                int currentPosition = 0;

                for (int i = 0; i < diskMap.Length; i++)
                {
                    int length = diskMap[i] - '0';
                    if (isFile)
                    {
                        var fileId = i / 2;
                        Files.Add(new FileBlock(fileId, currentPosition, length));
                        Output.Append(new string(fileId.ToString()[0], length));
                    }
                    else
                    {
                        FreeSpace.Enqueue(new FreeBlock(currentPosition, length));
                        Output.Append(new string('.', length));
                    }

                    currentPosition += length;
                    isFile = !isFile;
                }
            }

            public void MoveFilesToFreeSpacePart1()
            {
                var movedFiles = new List<FileBlock>();
                while (FreeSpace.Count > 0)
                {
                    var firstSpace = FreeSpace.Dequeue();

                    if (firstSpace.Start > Files.Last().Start)
                    {
                        break;
                    }

                    int currentStart = firstSpace.Start;
                    int remainingSpace = firstSpace.Length;
                    while (remainingSpace > 0 && Files.Any())
                    {
                        var lastFile = Files.Last();
                        int toMove = Math.Min(remainingSpace, lastFile.Length);
                        movedFiles.Add(new FileBlock(lastFile.Id, currentStart, toMove));
                        currentStart += toMove;

                        remainingSpace -= toMove;

                        if (toMove == lastFile.Length)
                        {
                            Files.RemoveAt(Files.Count - 1);
                        }
                        else
                        {
                            Files[Files.Count - 1] = new FileBlock(lastFile.Id, lastFile.Start, lastFile.Length - toMove);
                        }
                    }
                }

                Files.AddRange(movedFiles);
                Files = Files.OrderBy(f => f.Start).ToList();
            }

            public void MoveFilesToFreeSpacePart2()
            {
                var movedFiles = new List<FileBlock>();
                var updatedFreeSpaces = new Queue<FreeBlock>();

                for (int fileIndex = Files.Count - 1; fileIndex >= 0; fileIndex--)
                {
                    var fileToMove = Files[fileIndex];
                    bool fileMoved = false;

                    var freeSpaceList = FreeSpace.ToList();
                    for (int i = 0; i < freeSpaceList.Count; i++)
                    {
                        var freeBlock = freeSpaceList[i];
                        if (fileToMove.Length <= freeBlock.Length && fileToMove.Start > freeBlock.Start)
                        {
                            movedFiles.Add(new FileBlock(fileToMove.Id, freeBlock.Start, fileToMove.Length));
                            Files.RemoveAt(fileIndex);

                            freeBlock.Length -= fileToMove.Length;
                            freeBlock.Start += fileToMove.Length;

                            FreeSpace = new Queue<FreeBlock>(freeSpaceList.Where(fs => fs.Length > 0));

                            fileMoved = true;
                            break;
                        }
                    }

                    if (!fileMoved)
                    {
                        continue;
                    }
                }

                Files.AddRange(movedFiles);
                Files = Files.OrderBy(f => f.Start).ToList();
            }

            public long CalculateTotalSum()
            {
                long totalSum = 0;

                foreach (var file in Files)
                {
                    for (int blockId = 0; blockId < file.Length; blockId++)
                    {
                        totalSum += (file.Start + blockId) * file.Id;
                    }
                }

                return totalSum;
            }
        }

        public int SolvePart1(string[] input)
        {
            var diskMap = new DiskMap(input[0]);
            diskMap.MoveFilesToFreeSpacePart1();

            long totalSum = diskMap.CalculateTotalSum();
            Console.WriteLine($"Part 1 (Long): {totalSum}");

            return (int)Math.Min(totalSum, int.MaxValue);
        }

        public int SolvePart2(string[] input)
        {
            var diskMap = new DiskMap(input[0]);
            diskMap.MoveFilesToFreeSpacePart2();

            long totalSum = diskMap.CalculateTotalSum();
            Console.WriteLine($"Part 2 (Long): {totalSum}");

            return (int)Math.Min(totalSum, int.MaxValue);
        }

        public static void Solve()
        {
            string inputPath = "Days/Day9/input.txt";
            string[] lines = File.ReadAllLines(inputPath);

            var solution = new Solution();

            Console.WriteLine("Part 1 (Max Integer): " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2 (Max Integer): " + solution.SolvePart2(lines));
        }
    }
}
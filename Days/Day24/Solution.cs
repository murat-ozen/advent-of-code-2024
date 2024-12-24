using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Days.Day24
{
    public class Solution : ISolver
    {
        private record Gate(string Input1, string Op, string Input2, string Output);
        
        private static (Dictionary<string, int>, List<Gate>) ReadAndParseInput(string[] input)
        {
            var initialValues = new Dictionary<string, int>();
            var gates = new List<Gate>();
            var parsingGates = false;

            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    parsingGates = true;
                    continue;
                }

                if (!parsingGates)
                {
                    var parts = line.Split(':');
                    var wire = parts[0].Trim();
                    var value = int.Parse(parts[1].Trim());
                    initialValues[wire] = value;
                }
                else
                {
                    var parts = line.Split("->");
                    var outputWire = parts[1].Trim();
                    var inputs = parts[0].Trim().Split(' ');

                    gates.Add(new Gate(inputs[0], inputs[1], inputs[2], outputWire));
                }
            }

            return (initialValues, gates);
        }

        private static Dictionary<string, int> CalculateWireValues(Dictionary<string, int> initialValues, List<Gate> gates)
        {
            var wireValues = new Dictionary<string, int>(initialValues);
            var operations = new Queue<Gate>(gates);

            while (operations.Count > 0)
            {
                var gate = operations.Dequeue();

                if (wireValues.ContainsKey(gate.Input1) && wireValues.ContainsKey(gate.Input2))
                {
                    wireValues[gate.Output] = ProcessGate(gate.Op, wireValues[gate.Input1], wireValues[gate.Input2]);
                }
                else
                {
                    operations.Enqueue(gate);
                }
            }

            return wireValues;
        }

        private static HashSet<string> FindSwappedWires(Dictionary<string, int> initialValues, List<Gate> gates)
        {
            var swappedWires = new HashSet<string>();
            var highestZWire = gates.Where(g => g.Output.StartsWith("z"))
                                     .OrderByDescending(g => int.Parse(g.Output[1..]))
                                     .First().Output;

            foreach (var gate in gates)
            {
                if (gate.Output.StartsWith("z") && gate.Op != "XOR" && gate.Output != highestZWire)
                {
                    swappedWires.Add(gate.Output);
                }

                if (gate.Op == "XOR" && !gate.Output.StartsWith("x") && !gate.Output.StartsWith("y") && !gate.Output.StartsWith("z")
                    && !gate.Input1.StartsWith("x") && !gate.Input1.StartsWith("y") && !gate.Input2.StartsWith("z"))
                {
                    swappedWires.Add(gate.Output);
                }

                if (gate.Op == "AND" && gate.Input1 != "x00" && gate.Input2 != "x00")
                {
                    foreach (var subGate in gates)
                    {
                        if ((gate.Output == subGate.Input1 || gate.Output == subGate.Input2) && subGate.Op != "OR")
                        {
                            swappedWires.Add(gate.Output);
                        }
                    }
                }

                if (gate.Op == "XOR")
                {
                    foreach (var subGate in gates)
                    {
                        if ((gate.Output == subGate.Input1 || gate.Output == subGate.Input2) && subGate.Op == "OR")
                        {
                            swappedWires.Add(gate.Output);
                        }
                    }
                }
            }

            return swappedWires;
        }

        private static int ProcessGate(string op, int input1, int input2) => op switch
        {
            "AND" => input1 & input2,
            "OR" => input1 | input2,
            "XOR" => input1 ^ input2,
            _ => throw new ArgumentException("Invalid gate operation")
        };

        private static long CalculateZWiresValue(Dictionary<string, int> wireValues)
        {
            var zWires = wireValues.Keys
                .Where(k => k.StartsWith('z'))
                .OrderByDescending(k => int.Parse(k[1..]))
                .ToList();

            long result = 0;
            foreach (var wire in zWires)
            {
                result = (result << 1) | (uint)((byte)wireValues[wire]);
            }

            return result;
        }

        public int SolvePart1(string[] input)
        {
            var (initialValues, gates) = ReadAndParseInput(input);
            var wireValues = CalculateWireValues(initialValues, gates);
            
            var result = CalculateZWiresValue(wireValues);
            Console.WriteLine($"Part 1 (Long): {result}");
            return 0;
        }

        public int SolvePart2(string[] input)
        {
            var (initialValues, gates) = ReadAndParseInput(input);

            var swappedWires = FindSwappedWires(initialValues, gates);

            var result = string.Join(",", swappedWires.OrderBy(w => w));
            Console.WriteLine($"Part 2 (String): {result}");
            return 0;
        }

        public static void Solve()
        {
            string inputPath = "Days/Day24/input.txt";
            string[] lines = File.ReadAllLines(inputPath);

            var solution = new Solution();
            Console.WriteLine("Part 1: " + solution.SolvePart1(lines));
            Console.WriteLine("Part 2: " + solution.SolvePart2(lines));
        }
    }
}
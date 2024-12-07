using System.Reflection;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Lütfen bir gün belirtin. Örnek: dotnet run day1");
            return;
        }

        string dayNumber = args[0].ToLower();
        
        try
        {
            string solverTypeName = $"AdventOfCode2024.Days.{dayNumber.Substring(0, 1).ToUpper() + dayNumber.Substring(1)}.Solution";
            
            Type? solverType = Type.GetType(solverTypeName);
            
            if (solverType == null)
            {
                Console.WriteLine($"{dayNumber} için çözüm sınıfı bulunamadı.");
                return;
            }

            MethodInfo? solveMethod = solverType.GetMethod("Solve", BindingFlags.Public | BindingFlags.Static);
            
            if (solveMethod != null)
            {
                solveMethod.Invoke(null, null);
            }
            else
            {
                Console.WriteLine($"{dayNumber} için Solve metodu bulunamadı.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata oluştu: {ex.Message}");
        }
    }
}
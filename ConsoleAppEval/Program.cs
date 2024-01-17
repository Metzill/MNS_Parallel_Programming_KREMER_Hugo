using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        Task<long> sumTask = CalculateSumAsync(1, 3000);
        long sumResult = await sumTask;

        string filePath1 = "Eval_file1.txt";
        string filePath2 = "Eval_file2.txt";
        Task<int> wordCountTask1 = CountWordsAsync(filePath1);
        Task<int> wordCountTask2 = CountWordsAsync(filePath2);
        Task<int> loremCountTask1 = CountWordOccurrencesAsync(filePath1, "Lorem");
        Task<int> loremCountTask2 = CountWordOccurrencesAsync(filePath2, "Lorem");
        await Task.WhenAll(wordCountTask1, wordCountTask2, loremCountTask1, loremCountTask2);

        int wordCountFile1 = wordCountTask1.Result;
        int wordCountFile2 = wordCountTask2.Result;
        int loremCountFile1 = loremCountTask1.Result;
        int loremCountFile2 = loremCountTask2.Result;
        long sumAll = sumResult + wordCountFile1 + wordCountFile2 + loremCountFile1 + loremCountFile2;

        stopwatch.Stop();
        double elapsedTimeMilliseconds = stopwatch.Elapsed.TotalMilliseconds;

        // Afficher les résultats
        Console.WriteLine($"Somme des nombres de 1 à 3000 : {sumResult}");
        Console.WriteLine($"Nombre de mots dans {filePath1}: {wordCountFile1}");
        Console.WriteLine($"Nombre de 'Lorem' dans {filePath1}: {loremCountFile1}");
        Console.WriteLine($"Nombre de mots dans {filePath2}: {wordCountFile2}");
        Console.WriteLine($"Nombre de 'Lorem' dans {filePath2}: {loremCountFile2}");
        Console.WriteLine($"Somme de tous les résultats: {sumAll}");
        Console.WriteLine($"Temps d'exécution total : {elapsedTimeMilliseconds} ms");
    }

    static async Task<long> CalculateSumAsync(int start, int end)
    {
        return await Task.Run(() =>
        {
            long sum = 0;
            for (int i = start; i <= end; i++)
            {
                sum += i;
            }
            return sum;
        });
    }

    static async Task<int> CountWordsAsync(string filePath)
    {
        try
        {
            string content = await ReadFileAsync(filePath);

            string[] words = content.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            return words.Length;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Une erreur s'est produite lors de la lecture du fichier {filePath}: {ex.Message}");
            return 0;
        }
    }

    static async Task<int> CountWordOccurrencesAsync(string filePath, string targetWord)
    {
        try
        {
            string content = await ReadFileAsync(filePath);

            int count = content.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                               .Count(word => word.Equals(targetWord, StringComparison.OrdinalIgnoreCase));

            return count;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Une erreur s'est produite lors de la lecture du fichier {filePath}: {ex.Message}");
            return 0;
        }
    }

    static async Task<string> ReadFileAsync(string filePath)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            return await reader.ReadToEndAsync();
        }
    }
}

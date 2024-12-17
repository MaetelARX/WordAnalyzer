using System;
using System.Text;

namespace WordAnalyzer.Threaded_Version
{
    public class EngineT
    {
        public void Run()
        {
            Console.OutputEncoding = Encoding.UTF8;
            string filePath = @"../../../../filePath/Kristin-Hannah_-_Sydboven_pyt_-_11900-b.txt";
            char[] separators = { ' ', ',', '.', '?', '!', ';', ':', '-', '(', ')', '[', ']', '{', '}', '„', '“', '"', '_', '…', '\n', '\r' };

            string book = File.ReadAllText(filePath);
            string[] content = book.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            var finder = new Finder();

            Console.WriteLine("--- Multi-Threaded Version ---");
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            finder.ProcessText(content, useThreads: true);

            stopwatch.Stop();
            PrintResults(finder, stopwatch.ElapsedMilliseconds);
        }

        private void PrintResults(Finder finder, long time)
        {
            Console.WriteLine($"Number of words: {finder.WordCount}");
            Console.WriteLine($"Shortest word: {finder.ShortestWord}");
            Console.WriteLine($"Longest word: {finder.LongestWord}");
            Console.WriteLine($"Average word length: {finder.AverageWordLength:F2}");
            Console.WriteLine("Most common words: " + string.Join(", ", finder.MostCommonWords));
            Console.WriteLine("Least common words: " + string.Join(", ", finder.LeastCommonWords));
            Console.WriteLine($"Execution Time: {time} ms");
        }
    }
}

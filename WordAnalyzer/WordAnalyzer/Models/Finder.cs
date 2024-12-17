using System;
using System.Collections.Generic;
using System.Threading;
using WordAnalyzer.Models;

namespace WordAnalyzer
{
    public class Finder : IFinder
    {
        private readonly object lockObject = new object();

        public int WordCount { get; private set; }
        public string ShortestWord { get; private set; }
        public string LongestWord { get; private set; }
        public double AverageWordLength { get; private set; }
        public List<string> MostCommonWords { get; private set; }
        public List<string> LeastCommonWords { get; private set; }

        public void ProcessText(string[] content, bool useThreads = false)
        {
            if (useThreads)
            {
                ProcessWithThreads(content);
            }
            else
            {
                ProcessSequentially(content);
            }
        }

        private void ProcessSequentially(string[] content)
        {
            WordCount = CountWords(content);
            ShortestWord = FindShortestWord(content);
            LongestWord = FindLongestWord(content);
            AverageWordLength = CalculateAverageWordLength(content);
            MostCommonWords = FindTopWords(content, 5, true);
            LeastCommonWords = FindTopWords(content, 5, false);
        }

        private void ProcessWithThreads(string[] content)
        {
            Thread t1 = new Thread(() => WordCount = CountWords(content));
            Thread t2 = new Thread(() => ShortestWord = FindShortestWord(content));
            Thread t3 = new Thread(() => LongestWord = FindLongestWord(content));
            Thread t4 = new Thread(() => AverageWordLength = CalculateAverageWordLength(content));
            Thread t5 = new Thread(() => MostCommonWords = FindTopWords(content, 5, true));
            Thread t6 = new Thread(() => LeastCommonWords = FindTopWords(content, 5, false));

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();
            t6.Start();

            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();
            t5.Join();
            t6.Join();
        }

        public int CountWords(string[] book)
        {
            int count = 0;
            foreach (string word in book)
            {
                string cleanWord = CleanWord(word);
                if (cleanWord.Length >= 3)
                {
                    count++;
                }
            }
            return count;
        }

        public string FindShortestWord(string[] book)
        {
            string shortest = null;
            foreach (string word in book)
            {
                string cleanWord = CleanWord(word);
                if (cleanWord.Length >= 3 && (shortest == null || cleanWord.Length < shortest.Length))
                {
                    shortest = cleanWord;
                }
            }
            return shortest;
        }

        public string FindLongestWord(string[] book)
        {
            string longest = null;
            foreach (string word in book)
            {
                string cleanWord = CleanWord(word);
                if (cleanWord.Length >= 3 && (longest == null || cleanWord.Length > longest.Length))
                {
                    longest = cleanWord;
                }
            }
            return longest;
        }

        public double CalculateAverageWordLength(string[] book)
        {
            int totalLength = 0, count = 0;
            foreach (string word in book)
            {
                string cleanWord = CleanWord(word);
                if (cleanWord.Length >= 3)
                {
                    totalLength += cleanWord.Length;
                    count++;
                }
            }
            return count > 0 ? (double)totalLength / count : 0;
        }

        public List<string> FindTopWords(string[] book, int count, bool mostCommon)
        {
            Dictionary<string, int> wordCounts = new Dictionary<string, int>();

            foreach (string word in book)
            {
                string cleanWord = CleanWord(word);
                if (cleanWord.Length >= 3)
                {
                    if (wordCounts.ContainsKey(cleanWord))
                    {
                        lock (lockObject)
                        {
                            wordCounts[cleanWord]++;
                        }
                    }
                    else
                    {
                        lock (lockObject)
                        {
                            wordCounts[cleanWord] = 1;
                        }
                    }
                }
            }
            List<string> topWords = new List<string>();
            while (topWords.Count < count && wordCounts.Count > 0)
            {
                string selectedWord = null;
                int selectedCount = mostCommon ? 0 : int.MaxValue;

                foreach (var pair in wordCounts)
                {
                    if ((mostCommon && pair.Value > selectedCount) ||
                        (!mostCommon && pair.Value < selectedCount))
                    {
                        selectedCount = pair.Value;
                        selectedWord = pair.Key;
                    }
                }

                if (selectedWord != null)
                {
                    topWords.Add(selectedWord);
                    wordCounts.Remove(selectedWord);
                }
            }

            return topWords;
        }

        private string CleanWord(string word)
        {
            string cleaned = word.Trim().Trim(',', '.', '!', '?', '-', ':', ';', '(', ')', '[', ']', '{', '}', '"', '„', '“', '…');
            cleaned = cleaned.Replace("...", "");
            return cleaned;
        }
    }
}

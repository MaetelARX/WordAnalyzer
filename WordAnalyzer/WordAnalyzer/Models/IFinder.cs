using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordAnalyzer.Models
{
    public interface IFinder
    {
        public int CountWords(string[] book);
        public string FindShortestWord(string[] book);
        public string FindLongestWord(string[] book);
        public double CalculateAverageWordLength(string[] book);
        public List<string> FindTopWords(string[] book, int count, bool mostCommon);
    }
}

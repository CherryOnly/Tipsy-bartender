using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipsy_bartender
{
    public class BayesianCustomerMoodPredictor
    {
        private readonly string _dataFilePath;
        private readonly Dictionary<string, int> _categoryCounts = new Dictionary<string, int>();
        private readonly Dictionary<string, Dictionary<string, int>> _wordCounts = new Dictionary<string, Dictionary<string, int>>();
        private readonly HashSet<string> _uniqueWords = new HashSet<string>();

        public BayesianCustomerMoodPredictor(string dataFilePath)
        {
            _dataFilePath = dataFilePath;
            LoadData();
        }

        private void LoadData()
        {
            var lines = File.ReadAllLines(_dataFilePath);

            foreach (var line in lines)
            {
                var parts = line.Split('\t');

                if (parts.Length != 2)
                {
                    Console.WriteLine($"Invalid data format in line '{line}'");
                    continue;
                }

                var category = parts[0];
                var text = parts[1].ToLowerInvariant();

                if (!_categoryCounts.ContainsKey(category))
                {
                    _categoryCounts[category] = 0;
                    _wordCounts[category] = new Dictionary<string, int>();
                }

                _categoryCounts[category]++;
                var words = text.Split(' ');

                foreach (var word in words)
                {
                    if (string.IsNullOrWhiteSpace(word))
                    {
                        continue;
                    }

                    if (!_wordCounts[category].ContainsKey(word))
                    {
                        _wordCounts[category][word] = 0;
                    }

                    _wordCounts[category][word]++;
                    _uniqueWords.Add(word);
                }
            }
        }

        private double CalculateLogProbability(string category, string text)
        {
            var words = text.Split(' ');

            var logProb = Math.Log((double)_categoryCounts[category] / _categoryCounts.Values.Sum());

            foreach (var word in _uniqueWords)
            {
                var wordCount = _wordCounts[category].ContainsKey(word) ? _wordCounts[category][word] : 0;
                var wordProb = (double)(wordCount + 1) / (_wordCounts[category].Values.Sum() + _uniqueWords.Count);
                var wordIsPresent = words.Contains(word);
                logProb += Math.Log(wordIsPresent ? wordProb : 1 - wordProb);
            }

            return logProb;
        }

        public string PredictMood(string previousOrders)
        {
            var sadLogProb = CalculateLogProbability("Sad", previousOrders);
            var happyLogProb = CalculateLogProbability("Happy", previousOrders);

            if (sadLogProb > happyLogProb)
            {
                return "Sad";
            }
            else
            {
                return "Happy";
            }
        }
    }
}

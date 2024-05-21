namespace WordCounter
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using WordCounter.Abstraction;

    public class SequentialWordCounter : IWordCounter
    {
        public (IDictionary<string, int> wordCounts, IDictionary<string, IDictionary<string, int>> wordFollowers) CountWords(string path)
        {
            var wordCounts = new Dictionary<string, int>();
            var wordFollowers = new Dictionary<string, Dictionary<string, int>>();
            var separators = new char[] { ' ', '\t', '\r', '\n' };

            foreach (var line in File.ReadLines(path, Encoding.UTF8))
            {
                var words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < words.Length; i++)
                {
                    var normalizedWord = Regex.Replace(words[i], @"\p{P}", "").ToLowerInvariant();
                    if (string.IsNullOrEmpty(normalizedWord))
                    {
                        continue;
                    }

                    if (wordCounts.ContainsKey(normalizedWord))
                    {
                        wordCounts[normalizedWord]++;
                    }
                    else
                    {
                        wordCounts[normalizedWord] = 1;
                    }

                    if (i < words.Length - 1)
                    {
                        var nextWord = Regex.Replace(words[i + 1], @"\p{P}", "").ToLowerInvariant(); // removing punctuation
                        if (string.IsNullOrEmpty(nextWord)) continue;

                        if (!wordFollowers.ContainsKey(normalizedWord))
                        {
                            wordFollowers[normalizedWord] = new Dictionary<string, int>();
                        }

                        if (wordFollowers[normalizedWord].ContainsKey(nextWord))
                        {
                            wordFollowers[normalizedWord][nextWord]++;
                        }
                        else
                        {
                            wordFollowers[normalizedWord][nextWord] = 1;
                        }
                    }
                }
            }

            return (wordCounts, wordFollowers.ToDictionary(kvp => kvp.Key, kvp => (IDictionary<string, int>)kvp.Value));
        }
    }

}

using System.Collections.Concurrent;
using System.Text;
using System.Text.RegularExpressions;
using WordCounter.Abstraction;

public class ParallelWordCounter : IWordCounter
{
    public (IDictionary<string, int> wordCounts, IDictionary<string, IDictionary<string, int>> wordFollowers) CountWords(string path)
    {
        var wordCounts = new ConcurrentDictionary<string, int>();
        var wordFollowers = new ConcurrentDictionary<string, ConcurrentDictionary<string, int>>();
        var separators = new char[] { ' ', '\t', '\r', '\n' };

        Parallel.ForEach(File.ReadLines(path, Encoding.UTF8), line =>
        {
            var words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                var normalizedWord = Regex.Replace(words[i], @"\p{P}", "").ToLowerInvariant();
                if (string.IsNullOrEmpty(normalizedWord)) continue;

                wordCounts.AddOrUpdate(normalizedWord, 1, (_, count) => count + 1);

                if (i < words.Length - 1)
                {
                    var nextWord = Regex.Replace(words[i + 1], @"\p{P}", "").ToLowerInvariant();
                    if (string.IsNullOrEmpty(nextWord)) continue;

                    var followers = wordFollowers.GetOrAdd(normalizedWord, new ConcurrentDictionary<string, int>());
                    followers.AddOrUpdate(nextWord, 1, (_, count) => count + 1);
                }
            }
        });

        return (wordCounts, wordFollowers.ToDictionary(kvp => kvp.Key, kvp => (IDictionary<string, int>)kvp.Value));
    }
}

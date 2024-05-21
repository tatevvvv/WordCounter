namespace WordCounter
{
    internal static class WordCounterHelper
    {
        public static void DisplayResults(IDictionary<string, int> wordCounts, IDictionary<string, IDictionary<string, int>> wordFollowers)
        {
            var topWords = wordCounts.OrderByDescending(kvp => kvp.Value).Take(20);

            foreach (var (word, count) in topWords)
            {
                Console.Write($"{word} ({count}):\t");

                if (wordFollowers.TryGetValue(word, out var followers))
                {
                    var topFollowers = followers.OrderByDescending(f => f.Value).Take(5);
                    foreach (var follower in topFollowers)
                    {
                        Console.Write($"{follower.Key}({follower.Value})\t");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}

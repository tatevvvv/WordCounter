namespace WordCounter.Abstraction
{
    public interface IWordCounter
    {
        (IDictionary<string, int> wordCounts, IDictionary<string, IDictionary<string, int>> wordFollowers) CountWords(string path);
    }

}

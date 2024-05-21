using Microsoft.Extensions.DependencyInjection;
using WordCounter.Abstraction;

namespace WordCounter
{
    public class Program
    {
        static void Main(string[] args)
        {
            var path = "Text.txt";

            var serviceProvider = new ServiceCollection()
            .AddSingleton<IFileCharactersitcs, FileCharacteristics>((serviceProvider) => new FileCharacteristics(path))
            .AddSingleton<WordCounterFactory>()
            .BuildServiceProvider();

            // Get the WordCounterFactory and ResultPrinter from the DI container
            var factory = serviceProvider.GetService<WordCounterFactory>();

            var wordCounter = factory.GetWordCounter();
            var (wordCounts, wordFollowers) = wordCounter.CountWords(path);
            WordCounterHelper.DisplayResults(wordCounts, wordFollowers);
        }
    }
}

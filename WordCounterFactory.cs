using WordCounter.Abstraction;

namespace WordCounter
{
    public class WordCounterFactory
    {
        private const long ParallelProcessingThreshold = 10 * 1024 * 1024; // 10 mb
        private readonly IFileCharactersitcs _fileCharacteristcs;

        public WordCounterFactory(IFileCharactersitcs fileCharactersitcs)
        {
            _fileCharacteristcs = fileCharactersitcs ?? throw new ArgumentNullException(nameof(fileCharactersitcs));
        }

        public IWordCounter GetWordCounter()
        {
            var fileSize = _fileCharacteristcs.getFileSize();
            if (fileSize > ParallelProcessingThreshold)
            {
                Console.WriteLine("Decided to use parallel counter");
                return new ParallelWordCounter();
            }
            else
            {
                Console.WriteLine("Decided to use sequential counter");
                return new SequentialWordCounter();
            }
        }
    }
}

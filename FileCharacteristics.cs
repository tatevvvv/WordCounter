using WordCounter.Abstraction;

namespace WordCounter
{
    internal class FileCharacteristics : IFileCharactersitcs
    {
        private readonly string _filePath;
        public FileCharacteristics(string filePath)
        {
            _filePath = filePath;
        }

        public long? getFileSize()
        {
            try
            {
                var fileInfo = new FileInfo(_filePath);
                return fileInfo.Length;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}

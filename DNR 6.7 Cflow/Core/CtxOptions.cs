using System.IO;
using DNR.Utils;

namespace DNR.Core
{
    public class CtxOptions
    {
        public CtxOptions(string filePath, ILogger logger)
        {
            Logger = logger;
            FilePath = filePath;
            OutputPath = Path.Combine(Path.GetDirectoryName(filePath)!,
                $"{Path.GetFileNameWithoutExtension(filePath)}-nocflow{Path.GetExtension(filePath)}");
        }

        public ILogger Logger { get; }
        public string FilePath { get; }
        public string OutputPath { get; }
    }
}
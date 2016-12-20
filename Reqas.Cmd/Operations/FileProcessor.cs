using Microsoft.Extensions.Options;
using Reqas.Cmd.Options;

namespace Reqas.Cmd.Operations
{
    public class FileProcessor
    {
        private readonly PathOptions _pathOptions;
        public FileProcessor(IOptions<PathOptions> pathOptions)
        {
            _pathOptions = pathOptions.Value;
        }
    }
}

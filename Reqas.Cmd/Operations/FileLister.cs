using Microsoft.Extensions.Options;
using Reqas.Cmd.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Reqas.Cmd.Operations
{
    public class FileLister
    {
        private readonly PathOptions _pathOptions;
        public FileLister(IOptions<PathOptions> pathOptions)
        {
            _pathOptions = pathOptions.Value;
        }

        public IEnumerable<string> Execute()
            => Directory.GetFiles(_pathOptions.BasePath, Constants.MarkdownExtensionSearchPattern, SearchOption.AllDirectories).Where(f => !Path.GetFileName(f).StartsWith(Constants.PartialFilePrefix));
    }
}

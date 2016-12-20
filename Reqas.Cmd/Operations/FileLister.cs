using Microsoft.Extensions.Options;
using Reqas.Cmd.Options;
using System.Collections.Generic;
using System.IO;

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
        {
            return Directory.EnumerateFiles(_pathOptions.BasePath, Constants.MarkdownExtensionSearchPattern, SearchOption.AllDirectories);
        }
    }
}

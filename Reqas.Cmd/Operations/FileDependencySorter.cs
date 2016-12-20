using Microsoft.Extensions.Options;
using Reqas.Cmd.Options;

namespace Reqas.Cmd.Operations
{
    public class FileDependencySorter
    {
        private readonly PathOptions _pathOptions;
        public FileDependencySorter(IOptions<PathOptions> pathOptions)
        {
            _pathOptions = pathOptions.Value;
        }
    }
}

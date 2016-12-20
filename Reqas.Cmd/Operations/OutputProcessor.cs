using Microsoft.Extensions.Options;
using Reqas.Cmd.Options;

namespace Reqas.Cmd.Operations
{
    public class OutputProcessor
    {
        private readonly PathOptions _pathOptions;
        public OutputProcessor(IOptions<PathOptions> pathOptions)
        {
            _pathOptions = pathOptions.Value;
        }
    }
}

using Microsoft.Extensions.Options;
using Reqas.Cmd.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Reqas.Cmd.Operations
{
    public class FileDependencyBuilder
    {
        private readonly PathOptions _pathOptions;
        public FileDependencyBuilder(IOptions<PathOptions> pathOptions)
        {
            _pathOptions = pathOptions.Value;
        }

        public Dictionary<string, string[]> Execute(IEnumerable<string> files)
        {
            var result = new Dictionary<string, string[]>();

            files
                .ToList()
                .ForEach(f =>
                {
                    var currentPath = Path.GetDirectoryName(f);
                    Func<string, string> buildDependencyPath =
                        x => Path.GetFullPath(Path.Combine(currentPath, x.Replace(Constants.DependencyPrefix, string.Empty)));

                    var dependencies = File.ReadAllLines(f)
                        .Where(x => x.StartsWith(Constants.DependencyPrefix))
                        .Select(buildDependencyPath)
                        .ToArray();

                    result.Add(f, dependencies);

                    Console.WriteLine($"F: {f}");
                    dependencies.ToList().ForEach(d => Console.WriteLine($" D: {d}"));
                });

            return result;
        }
    }
}

using Microsoft.Extensions.Options;
using Reqas.Cmd.Domain;
using Reqas.Cmd.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Reqas.Cmd.Operations
{
    public class FileDependencyBuilder
    {
        private readonly Func<string, string, string> _buildDependencyPath =
            (path, line) => Path.GetFullPath(Path.Combine(path, line.Replace(Constants.DependencyPrefix, string.Empty).Trim()));

        private readonly PathOptions _pathOptions;
        public FileDependencyBuilder(IOptions<PathOptions> pathOptions)
        {
            _pathOptions = pathOptions.Value;
        }

        public List<IncludedFile> Execute(IEnumerable<string> files)
        {
            return files.ToList().Select(x => BuildIncludedFile(string.Empty, x)).ToList();
        }

        public IncludedFile BuildIncludedFile(string includeName, string filePath)
        {
            var currentPath = Path.GetDirectoryName(filePath);
            var dependencyPaths = File.ReadAllLines(filePath)
                .Where(line => line.StartsWith(Constants.DependencyPrefix))
                .Select(line => new
                {
                    IncludeName = line,
                    DependencyPath = _buildDependencyPath(currentPath, line)
                })
                .ToArray();

            var dependencies = new List<IncludedFile>();
            foreach (var d in dependencyPaths)
            {
                dependencies.Add(BuildIncludedFile(d.IncludeName, d.DependencyPath));
            }

            return new IncludedFile(filePath, includeName, dependencies.ToArray());
        }
    }
}

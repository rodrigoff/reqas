using Microsoft.Extensions.Options;
using Reqas.Cmd.Domain;
using Reqas.Cmd.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Reqas.Cmd.Operations
{
    public class OutputProcessor
    {
        private readonly PathOptions _pathOptions;
        public OutputProcessor(IOptions<PathOptions> pathOptions)
        {
            _pathOptions = pathOptions.Value;
        }

        public void Execute(List<IncludedFile> dependencyGraph)
        {
            foreach (var entry in dependencyGraph)
            {
                var s = GenerateNewFile(entry.FileName, entry.Dependencies);
            }
        }

        private List<string> GenerateNewFile(string fileName, IncludedFile[] dependencies)
        {
            var file = ReadFile(fileName);
            foreach (var dependency in dependencies)
            {
                if (!dependency.Dependencies.Any())
                {
                    file.AddRange(ReadFile(dependency.FileName));
                }
                else
                {
                    var newFile = GenerateNewFile(dependency.FileName, dependency.Dependencies);
                    var processedFile = new List<string>();
                    foreach (var line in file)
                    {
                        if (!line.Contains(dependency.IncludeName))
                        {
                            processedFile.Add(line);
                        }
                        else
                        {
                            processedFile.AddRange(newFile);
                        }
                    }
                    file = processedFile;
                }
            }
            return file.Where(x => !x.StartsWith(Constants.DependencyPrefix)).ToList();
        }

        private List<string> ReadFile(string filePath) => File.ReadAllLines(filePath).ToList();
    }
}

using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Reqas.Cmd
{
    public class FileReader
    {
        private readonly PathOptions _pathOptions;
        public FileReader(IOptions<PathOptions> pathOptions)
        {
            _pathOptions = pathOptions.Value;
        }

        public Dictionary<string, string[]> BuildDependencyDictionary()
        {
            var dict = new Dictionary<string, string[]>();

            var markdownFiles = Directory.EnumerateFiles(_pathOptions.BasePath, "*.md", SearchOption.AllDirectories);
            Console.WriteLine($"Markdown files: {markdownFiles.Count()}");

            markdownFiles
                .ToList()
                .ForEach(f =>
                {
                    var currentPath = Path.GetDirectoryName(f);

                    var dependencies = File.ReadAllLines(f)
                        .Where(x => x.StartsWith("::"))
                        .Select(x => Path.GetFullPath(Path.Combine(currentPath, x.Replace("::", string.Empty))))
                        .ToArray();

                    dict.Add(f, dependencies);

                    Console.WriteLine($"F: {f}");
                    dependencies.ToList().ForEach(d => Console.WriteLine($" D: {d}"));
                });

            return dict;
        }
    }
}

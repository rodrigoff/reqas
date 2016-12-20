using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Reqas.Cmd
{
    public class FileReader
    {
        public static Dictionary<string, string[]> BuildDependencyDictionary(string basePath)
        {
            var dict = new Dictionary<string, string[]>();

            var markdownFiles = Directory.EnumerateFiles(basePath, "*.md", SearchOption.AllDirectories);
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

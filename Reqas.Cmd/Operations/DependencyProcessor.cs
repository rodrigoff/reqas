using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Reqas.Cmd.Domain;
using Reqas.Cmd.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Reqas.Cmd.Operations
{
    public class DependencyProcessor
    {
        private readonly PathOptions _pathOptions;
        private readonly AssetsProcessor _assetsProcessor;
        public DependencyProcessor(IOptions<PathOptions> pathOptions, AssetsProcessor assetsProcessor)
        {
            _pathOptions = pathOptions.Value;
            _assetsProcessor = assetsProcessor;
        }

        public IEnumerable<ProcessedFile> Execute(List<IncludedFile> dependencyGraph)
        {
            var result = new List<ProcessedFile>();
            foreach (var entry in dependencyGraph)
            {
                var currentFolder = Path.GetDirectoryName(entry.FileName);
                var configFilePath = $"{currentFolder}\\{Constants.ConfigFile}";
                System.Console.WriteLine(configFilePath);
                if (!File.Exists(configFilePath))
                {
                    throw new FileNotFoundException($"Configuration file not found at {currentFolder}");
                }


                var configuration = JsonConvert.DeserializeObject<PathOptions>(File.ReadAllText(configFilePath));
                var outputPath = $"{_pathOptions.Output}\\{Path.GetFileNameWithoutExtension(configuration.Output)}";
                Directory.CreateDirectory($"{outputPath}\\Assets");
                Directory.CreateDirectory(outputPath);

                var fileContent = GenerateNewFile(entry.FileName, entry.Dependencies, outputPath);

                File.WriteAllLines(Path.Combine(outputPath, configuration.Output), fileContent);

                result.Add(new ProcessedFile(entry.FileName, fileContent));
            }

            return result;
        }

        private List<string> GenerateNewFile(string fileName, IncludedFile[] dependencies, string outputPath)
        {
            var file = ReadFile(fileName);
            foreach (var dependency in dependencies)
            {
                if (!dependency.Dependencies.Any())
                {
                    var readFile = ReadFile(dependency.FileName);
                    file.AddRange(_assetsProcessor.Execute(dependency.FileName, readFile, outputPath));
                }
                else
                {
                    var newFile = GenerateNewFile(dependency.FileName, dependency.Dependencies, outputPath);
                    newFile = _assetsProcessor.Execute(dependency.FileName, newFile, outputPath);
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

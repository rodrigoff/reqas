using System.Collections.Generic;
using System;
using System.IO;

namespace Reqas.Cmd.Operations
{
    public class AssetsProcessor
    {
        public List<string> Execute(string fileName, List<string> fileContent, string outputPath)
        {
            var processedAssets = new Dictionary<string, string>();

            var assetsFolder = Path.Combine(Path.GetDirectoryName(fileName), "Assets");
            if (!Directory.Exists(assetsFolder))
            {
                return fileContent;
            }

            var assets = Directory.GetFiles(assetsFolder);
            foreach (var asset in assets)
            {
                var newAssetName = $"{Path.GetFileNameWithoutExtension(asset)}_{Guid.NewGuid()}{Path.GetExtension(asset)}";
                File.Copy(asset, Path.Combine(outputPath, "Assets", newAssetName));

                processedAssets.Add(Path.GetFileName(asset), newAssetName);
            }

            var processedFile = new List<string>();
            foreach (var line in fileContent)
            {
                foreach (var asset in processedAssets)
                {
                    processedFile.Add(line.Replace(asset.Key, asset.Value));
                }
            }

            return processedFile;
        }
    }
}
using Microsoft.Extensions.Configuration;
using Reqas.Cmd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

class Program
{
    private static IConfigurationRoot _configuration;
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        BuildConfiguration();

        var basePath = _configuration[Constants.BasePathKey];

        Console.WriteLine($"BasePath: {basePath}");

        Console.WriteLine(string.Empty);

        var dependencyDictionary = FileReader.BuildDependencyDictionary(basePath);
    }

    private static void BuildConfiguration()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(Constants.ConfigFile);

        _configuration = config.Build();
    }
}
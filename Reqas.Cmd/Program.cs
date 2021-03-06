﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reqas.Cmd;
using Reqas.Cmd.Operations;
using Reqas.Cmd.Options;
using System;
using System.IO;
using System.Text;

class Program
{
    public static IConfigurationRoot Configuration { get; set; }

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        BuildConfiguration();
        var serviceProvider = BuildServiceProvider();

        var fileLister = serviceProvider.GetService<FileLister>();
        var fileDependencyBuilder = serviceProvider.GetService<FileDependencyBuilder>();
        var dependencyProcessor = serviceProvider.GetService<DependencyProcessor>();
        var assetsProcessor = serviceProvider.GetService<AssetsProcessor>();

        var files = fileLister.Execute();
        var dependencyGraph = fileDependencyBuilder.Execute(files);
        var convertedFiles = dependencyProcessor.Execute(dependencyGraph);
    }

    private static void BuildConfiguration()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(Constants.ConfigFile);

        Configuration = config.Build();
    }

    private static IServiceProvider BuildServiceProvider()
    {
        var serviceCollection = new ServiceCollection();

        ConfigureServices(serviceCollection);

        return serviceCollection.BuildServiceProvider();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddOptions();
        services.Configure<PathOptions>(Configuration);

        services.AddScoped<FileLister>();
        services.AddScoped<FileDependencyBuilder>();
        services.AddScoped<DependencyProcessor>();
        services.AddScoped<AssetsProcessor>();
    }
}
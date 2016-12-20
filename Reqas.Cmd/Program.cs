using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reqas.Cmd;
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
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var fileReader = serviceProvider.GetService<FileReader>();


        var basePath = Configuration[Constants.BasePathKey];

        Console.WriteLine($"BasePath: {basePath}");

        Console.WriteLine(string.Empty);

        var dependencyDictionary = fileReader.BuildDependencyDictionary();
    }

    private static void BuildConfiguration()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(Constants.ConfigFile);

        Configuration = config.Build();
    }
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddOptions();

        services.Configure<PathOptions>(Configuration);

        services.AddScoped<FileReader>();
        services.AddScoped<FileProcessor>();
    }
}
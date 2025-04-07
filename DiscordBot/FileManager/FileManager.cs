using Discord.Logger;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Discord.FileManager;

public class FileManager
{
    private readonly IDeserializer _deserializer;
    private readonly ISerializer _serializer;

    public FileManager()
    {
        _deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        _serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
    }

    public async Task InitializeFileManager()
    {
        const string filePath = "config.yml";
        await ConsoleLogger.LogAsync("FileManager", "Loading YML file...", ConsoleLogger.LogSeverityLevels.LOADING);

        try
        {
            var config = await LoadYamlAsync<Config>(filePath);
            await ConsoleLogger.LogAsync("FileManager", "Loaded YML file successfully!", ConsoleLogger.LogSeverityLevels.LOADING);
            
            config.Version++;
            var updatedFeatures = config.Features.ToList();
            config.Features = updatedFeatures.ToArray();
            
            await SaveYamlAsync(config, filePath);
            await ConsoleLogger.LogAsync("FileManager", "Updated and saved YML file successfully!", ConsoleLogger.LogSeverityLevels.OK);
        }
        catch (FileNotFoundException ex)
        {
            await ConsoleLogger.LogAsync("FileManager", $"File not found: {ex.Message}", ConsoleLogger.LogSeverityLevels.ERROR);
        }
        catch (Exception ex)
        {
            await ConsoleLogger.LogAsync("FileManager", $"An error occurred: {ex.Message}", ConsoleLogger.LogSeverityLevels.FATAL);
            throw;
        }
    }
    
    public async Task<T> LoadYamlAsync<T>(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"YML file not found at path: {filePath}");

        var yamlContent = await File.ReadAllTextAsync(filePath);
        return _deserializer.Deserialize<T>(yamlContent);
    }


    public async Task SaveYamlAsync<T>(T data, string filePath)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data), "Cannot serialize a null object.");
        }

        var yamlContent = _serializer.Serialize(data);

        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        await File.WriteAllTextAsync(filePath, yamlContent);
        await ConsoleLogger.LogAsync("FileManager", "YML file saved successfully!", ConsoleLogger.LogSeverityLevels.OK);
    }
}
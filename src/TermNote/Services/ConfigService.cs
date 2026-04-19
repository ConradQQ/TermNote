namespace TermNote.Services;

using System.Text.Json;
using TermNote.Models;
using System.IO;

public class ConfigService
{
  private readonly string _filePath;

  public ConfigService(string? directory = null)
  {
    var xdgConfig = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
    var configBase = !string.IsNullOrEmpty(xdgConfig)
      ? xdgConfig
      : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config");

    var dir = directory ?? Path.Combine(configBase, "termnote");
    Directory.CreateDirectory(dir);
    _filePath = Path.Combine(dir, "config.json");
  }

  public Config Load()
  {

    if (!File.Exists(_filePath))
    {
      var newConfig = new Config();
      var options = new JsonSerializerOptions
      {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      };
      var json = JsonSerializer.Serialize(newConfig, options);
      File.WriteAllText(_filePath, json);

      return newConfig;
    }
    else
    {
      try
      {

        var json = File.ReadAllText(_filePath);
        var options = new JsonSerializerOptions
        {
          PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        return JsonSerializer.Deserialize<Config>(json, options) ?? new Config();
      } catch (JsonException ex)
      {
        Console.WriteLine(ex.Message);
        return new Config();
      }

    }

  }
  public string ConfigPath()
  {
    return _filePath;
  }
}

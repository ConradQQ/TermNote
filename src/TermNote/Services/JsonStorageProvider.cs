using System.Text.Json;
using TermNote.Models;

namespace TermNote.Services;

public class JsonStorageProvider : IStorageProvider
{
  // File path field
  private readonly string _filePath;

  // Constructor function
  public JsonStorageProvider(string? directory = null)
  {
      var xdgConfig = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
      var configBase = !string.IsNullOrEmpty(xdgConfig)
        ? xdgConfig
        : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config");

      var dir = directory ?? Path.Combine(configBase, "termnote");
      Directory.CreateDirectory(dir);
      _filePath = Path.Combine(dir, "notes.json");
    }

  // Save method
  public void Save(List<Note> notes)
  {
    var options = new JsonSerializerOptions
    {
      WriteIndented = true,
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    var json = JsonSerializer.Serialize(notes, options);
    File.WriteAllText(_filePath, json);
  }

  // Load Method
  public List<Note> Load()
  {
    if (!File.Exists(_filePath))
      return new List<Note>();

    try
    {
      var json = File.ReadAllText(_filePath);
      var options = new JsonSerializerOptions
      {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      };

      return JsonSerializer.Deserialize<List<Note>>(json, options)
          ?? new List<Note>();
    }
    catch (JsonException)
    {
      // Corrupted file — start fresh rather than crash
      return new List<Note>();
    }
  }

}


using System.Text.Json;
using System.Xml.Linq;
using TermNote.Models;

namespace TermNote.Services; 

public class NoteStore
{
  
  // Private fields _ the internal state of this object|
  // Outside code cannot see o rtouch these
  private readonly string _filePath;
  private readonly List<Note> _notes;

  // Static property _ belongs to the class itself, not to any instance
  // Figures out the right config directory for any OS
  public static string DefaultDirectory
  {
    get
    {
      var xdgConfig = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
      var configBase = !string.IsNullOrEmpty(xdgConfig)
        ? xdgConfig
        : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config");

      return Path.Combine(configBase, "TermNote");
    }
  }

  public NoteStore(string? directory = null)
  {
    var dir = directory ?? DefaultDirectory;
    Directory.CreateDirectory(dir); // Ensure the directory exists
    _filePath = Path.Combine(dir, "notes.json");
    _notes = Load();
  }

  public Note Add(string content)
  {
    var note = new Note(content);
    _notes.Add(note);
    Save();
    return note;
  }

  public bool Remove(string idOrPrefix)
  {
    var note = FindByIdPrefix(idOrPrefix);
    if (note is null) return false;

    _notes.Remove(note);
    Save();
    return true;
  }

  public bool Edit(string idOrPrefix, string newContent)
  {
    var note = FindByIdPrefix(idOrPrefix);
    if (note is null) return false;

    note.Content = newContent;
    Save();
    return true;
  }
  
  public IReadOnlyList<Note> GetAll()
  {
    return _notes
      .OrderByDescending(n => n.CreatedAt)
      .ToList()
      .AsReadOnly();
  }

  public int Count => _notes.Count;

  private Note? FindByIdPrefix(string prefix)
  {
    var matches = _notes
      .Where(n => n.Id.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
      .ToList();
    return matches.FirstOrDefault();

  }

  private void Save()
  {
    var options = new JsonSerializerOptions
    {
      WriteIndented = true,
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    var json = JsonSerializer.Serialize(_notes, options);
    File.WriteAllText(_filePath, json);
  }

    private List<Note> Load()
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
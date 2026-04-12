using TermNote.Models;

namespace TermNote.Services; 

public class NoteStore
{
  
  // Private fields _ the internal state of this object|
  // Outside code cannot see o rtouch these
  private readonly List<Note> _notes;
  private readonly IStorageProvider _storage;
  // Static property _ belongs to the class itself, not to any instance
  // Figures out the right config directory for any OS


  public NoteStore(IStorageProvider storage)
  {
    _notes = storage.Load();
    _storage = storage;

  }

  public Note Add(string content)
  {
    var note = new Note(content);
    _notes.Add(note);
    _storage.Save(_notes);
    return note;
  }

  public bool Remove(string idOrPrefix)
  {
    var note = FindByIdPrefix(idOrPrefix);
    if (note is null) return false;

    _notes.Remove(note);
    _storage.Save(_notes);
    return true;
  }

  public bool Edit(string idOrPrefix, string newContent)
  {
    var note = FindByIdPrefix(idOrPrefix);
    if (note is null) return false;

    note.Content = newContent;
    _storage.Save(_notes);
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

}
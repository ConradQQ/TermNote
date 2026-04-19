using TermNote.Rendering;
using TermNote.Services;
using TermNote.Models;
using System.Text.Json;

namespace TermNote.Commands;

public class CommandHandler
{
  private readonly NoteStore _store;
  private readonly IRenderer _renderer;
  private readonly Config _config;

  public CommandHandler(NoteStore store, IRenderer renderer, Config config)
  {
    _store = store;
    _renderer = renderer;
    _config = config;
  }

  public int Execute(string[] args)
  {

    if (args.Length == 0)
    {
      return HandleShow();
    }

    var command = args[0].ToLowerInvariant();
    var commandArgs = args[1..];

    return command switch
    {
      "add" or "a" => HandleAdd(commandArgs),
      "list" or "ls" => HandleList(),
      "remove" or "rm" => HandleRemove(commandArgs),
      "edit" or "e" => HandleEdit(commandArgs),
      "show" or "s" => HandleShow(),
      "clear" => HandleClear(),
      "config" => HandleConfig(),
      "help" or "h" or "--help" or "-h" => HandleHelp(),
      "version" or "--version" or "-v" => HandleVersion(),
      _ => HandleUnknown(command),
    };
  }

  // Method for showing current notes in the terminal
  private int HandleShow()
  {
    _renderer.Render(_store.GetAll());
    return 0;
  }

  // Method for adding a new note
  private int HandleAdd(string[] args)
  {
    if (args.Length == 0)
    {
      Console.Error.WriteLine("Usage: termnote add <content>");
      Console.Error.WriteLine(" Example: <termnote add Deploy hotfix to QA by EOD>");
      return 1;
    }

    var content = string.Join(' ', args);
    var note = _store.Add(content);

    Console.WriteLine($"  \x1b[32m✓\x1b[0m Note added:");
    _renderer.RenderInline(note);

    return 0;
  }

  // Method for listing out all notes with their IDs
  private int HandleList()
  {
    var notes = _store.GetAll();

    if (notes.Count == 0)
    {
      Console.WriteLine("  \x1b[2mNo notes. Add one with: termnote add <content>\x1b[0m");
      return 0;
    }

    Console.WriteLine();
    foreach (var note in notes)
    {
      Console.WriteLine($"  \x1b[90m{note.Id}\x1b[0m  {note.Content}  \x1b[2m{note.GetAge()}\x1b[0m");
    }
    Console.WriteLine();
    Console.WriteLine($"  \x1b[2m{notes.Count} note(s)\x1b[0m");
    Console.WriteLine();

    return 0;
  }

  // Method for removing a given note by its ID
  private int HandleRemove(string[] args)
  {
    if (args.Length == 0)
    {
      Console.Error.WriteLine("Usage: termnote rm <id>");
      Console.Error.WriteLine("  Tip: Use 'termnote list' to see note IDs.");
      return 1;
    }

    var id = args[0];
    if (_store.Remove(id))
    {
      Console.WriteLine("  \x1b[32m✓\x1b[0m Note removed.");
      return 0;
    }
    else
    {
      Console.Error.WriteLine($"  \x1b[31m✗\x1b[0m No note found matching '{id}'.");
      Console.Error.WriteLine("  Tip: Use 'termnote list' to see all note IDs.");
      return 1;
    }
  }

  // Method for editing a given note by its ID
  private int HandleEdit(string[] args)
  {
    if (args.Length < 2)
    {
      Console.Error.WriteLine("Usage: termnote edit <id> <new content>");
      return 1;
    }

    var id = args[0];
    var newContent = string.Join(' ', args[1..]);

    if (_store.Edit(id, newContent))
    {
      Console.WriteLine("  \x1b[32m✓\x1b[0m Note updated.");
      return 0;
    }
    else
    {
      Console.Error.WriteLine($"  \x1b[31m✗\x1b[0m No note found matching '{id}'.");
      return 1;
    }
  }

  // Method for clearing all notes
  private int HandleClear()
  {
    var count = _store.Count;
    if (count == 0)
    {
      Console.WriteLine("  \x1b[2mNo notes to clear.\x1b[0m");
      return 0;
    }

    var notes = _store.GetAll();
    foreach (var note in notes)
    {
      _store.Remove(note.Id);
    }

    Console.WriteLine($"  \x1b[32m✓\x1b[0m Cleared {count} note(s).");
    return 0;
  }

  // Method for printing config contents to the terminal
  private int HandleConfig()
  {
    var options = new JsonSerializerOptions
    {
      WriteIndented = true,
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
    string json = JsonSerializer.Serialize(_config, options);
    Console.WriteLine(json);
    return 0;
  }

  // Help command for CLI tool
  private static int HandleHelp()
  {
    Console.WriteLine();
    Console.WriteLine("  \x1b[1mtermnote\x1b[0m — Persistent sticky notes for your terminal");
    Console.WriteLine();
    Console.WriteLine("  Usage:");
    Console.WriteLine("    termnote                     Show notes (for shell integration)");
    Console.WriteLine("    termnote add <content>       Add a new note");
    Console.WriteLine("    termnote list                List all notes with full IDs");
    Console.WriteLine("    termnote rm <id>             Remove a note (supports partial IDs)");
    Console.WriteLine("    termnote edit <id> <content> Edit a note's content");
    Console.WriteLine("    termnote clear               Remove all notes");
    Console.WriteLine("    termnote help                Show this help");
    Console.WriteLine("    termnote version             Show version");
    Console.WriteLine();
    Console.WriteLine("  Shell integration (add to ~/.zshrc):");
    Console.WriteLine("    \x1b[2mtermnote show\x1b[0m");
    Console.WriteLine();
    return 0;
  }

  // Version command for CLI tool
  private static int HandleVersion()
  {
    var version = typeof(CommandHandler).Assembly.GetName().Version;
    Console.WriteLine($"  termnote {version?.ToString(3) ?? "0.1.0"}");
    return 0;
  }

  // Catch any unknown inputs
  private static int HandleUnknown(string command)
  {
    Console.Error.WriteLine($"  Unknown command: '{command}'");
    Console.Error.WriteLine("  Run 'termnote help' for usage information.");
    return 1;
  }

}

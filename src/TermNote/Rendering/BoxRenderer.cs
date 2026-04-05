using TermNote.Models;
namespace TermNote.Rendering;

public static class BoxRenderer
{
  // ANSI escape codes for styling
  private const string Reset = "\x1b[0m";
  private const string Bold = "\x1b[1m";
  private const string Dim = "\x1b[2m";
  private const string Cyan = "\x1b[36m";
  private const string Yellow = "\x1b[33m";
  private const string Green = "\x1b[32m";
  private const string White = "\x1b[37m";
  private const string BrightBlack = "\x1b[90m";


  // Box drawing characters
  private const char TopLeft = '╭';
  private const char TopRight = '╮';
  private const char BottomLeft = '╰';
  private const char BottomRight = '╯';
  private const char Horizontal = '─';
  private const char Vertical = '│';

  public static void Render(IReadOnlyList<Note> notes, int maxWidth = 60)
  {

    if (notes.Count == 0)
      return;

    var innerWidth = maxWidth - 4;

    var topBorder = $"{Dim}{TopLeft}{new string(Horizontal, maxWidth - 2)}{TopRight}{Reset}";
    var bottomBorder = $"{Dim}{BottomLeft}{new string(Horizontal, maxWidth - 2)}{BottomRight}{Reset}";
    var titleLine = FormatCentered($"📌 termnote ({notes.Count})", innerWidth);

    Console.WriteLine(topBorder);

    Console.WriteLine();
    Console.WriteLine($"{Dim}{Vertical}{Reset} {Bold}{Cyan}{titleLine}{Reset} {Dim}{Vertical}{Reset}");
    Console.WriteLine($"{Dim}{Vertical}{Reset} {new string(Horizontal, innerWidth)} {Dim}{Vertical}{Reset}");

    foreach (var note in notes)
    {
      var contentLine = FormatNote(note, innerWidth);
      Console.WriteLine($"{Dim}{Vertical}{Reset} {contentLine} {Dim}{Vertical}{Reset}");

      Console.WriteLine(bottomBorder);
    }
  }

  private static string FormatCentered(string text, int width)
  {
    var visibleLength = text.Length;
    if (visibleLength >= width) return text;

    var leftPad = (width - visibleLength) / 2;
    var rightPad = width - visibleLength - leftPad;

    return new string(' ', leftPad) + text + new string(' ', rightPad);
  }

  private static string FormatNote(Note note, int innerWidth)
  {
    var idTag = $"{BrightBlack}{note.Id[..4]}{Reset}";
    var age = $"{BrightBlack}{note.GetAge()}{Reset}";
    var suffix = $" {idTag} {Dim}·{Reset} {age}";
    var suffixVisible = $" {note.Id[..4]} · {note.GetAge()}";
    var maxContent = innerWidth - suffixVisible.Length;

    var content = note.Content.Length > maxContent
        ? note.Content[..(maxContent - 1)] + "…"
        : note.Content;

    var padding = innerWidth - content.Length - suffixVisible.Length;
    var pad = padding > 0 ? new string(' ', padding) : "";

    return $"{White}{content}{Reset}{pad}{suffix}";
  }

  public static void RenderInline(Note note)
  {
    Console.WriteLine($"  {BrightBlack}[{note.Id}]{Reset} {note.Content}");
  }

}

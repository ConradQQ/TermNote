using TermNote.Models;
namespace TermNote.Rendering;

public static class BoxRenderer
{
  public static string Render(Note note, int width = 50)
  {
    var contentLines = SplitContent(note.Content, width - 4); // Account for borders
    var age = note.GetAge();
    var header = $" {note.Id} - {age} ";
    var topBorder = "┌" + new string('─', width - 2) + "┐";
    var bottomBorder = "└" + new string('─', width - 2) + "┘";
    var renderedContent = contentLines.Select(line => $"│ {line.PadRight(width - 4)} │");
    return string.Join(Environment.NewLine, new[] { topBorder, header }.Concat(renderedContent).Concat(new[] { bottomBorder }));
  }

  private static IEnumerable<string> SplitContent(string content, int maxWidth)
  {
    var words = content.Split(' ');
    var line = "";
    foreach (var word in words)
    {
      if ((line + word).Length > maxWidth)
      {
        yield return line.TrimEnd();
        line = "";
      }
      line += word + " ";
    }
    if (!string.IsNullOrWhiteSpace(line))
      yield return line.TrimEnd();
  }
}
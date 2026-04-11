using TermNote.Models;
namespace TermNote.Rendering;

public class MinimalRenderer : IRenderer
{
  public void Render(IReadOnlyList<Note> notes, int maxWidth = 60)
  {
    if (notes.Count == 0)
      return; 


    var innerWidth = maxWidth - 4;

    foreach (var note in notes)
    {
      Console.WriteLine(note.Content);
    }

  }

  public void RenderInline(Note note)
  {
   Console.WriteLine(note.Content);
  }
}
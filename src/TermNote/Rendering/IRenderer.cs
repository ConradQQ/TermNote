using TermNote.Models;

namespace TermNote.Rendering;

public interface IRenderer
{
  void Render(IReadOnlyList<Note> notes, int maxWidth = 60);

  void RenderInline(Note note);
}
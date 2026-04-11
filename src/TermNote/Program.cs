using TermNote.Commands;
using TermNote.Services;
using TermNote.Rendering;

class Program
{
    static int Main(string[] args)
    {
        var store = new NoteStore();
        var minimalRenderer = new MinimalRenderer();
        var boxRenderer = new BoxRenderer();
        var handler = new CommandHandler(store, boxRenderer);
        return handler.Execute(args);
    }
}
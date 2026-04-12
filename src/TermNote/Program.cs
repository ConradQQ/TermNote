using TermNote.Commands;
using TermNote.Services;
using TermNote.Rendering;

class Program
{
    static int Main(string[] args)
    {
        var storage = new JsonStorageProvider();
        var store = new NoteStore(storage);
        var minimalRenderer = new MinimalRenderer();
        var boxRenderer = new BoxRenderer();
        var handler = new CommandHandler(store, boxRenderer);

        return handler.Execute(args);
    }
}
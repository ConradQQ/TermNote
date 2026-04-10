using TermNote.Commands;
using TermNote.Services;

class Program
{
    static int Main(string[] args)
    {
        var store = new NoteStore();
        var handler = new CommandHandler(store);
        return handler.Execute(args);
    }
}
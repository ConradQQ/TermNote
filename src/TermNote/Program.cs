using TermNote.Commands;
using TermNote.Services;
using TermNote.Rendering;

class Program
{
    static int Main(string[] args)
    {
        var storage = new JsonStorageProvider();
        var store = new NoteStore(storage);
        var configService = new ConfigService();
        var config = configService.Load();

        IRenderer renderer = config.Renderer == "box"
        ? new BoxRenderer()
        : new MinimalRenderer();
        var handler = new CommandHandler(store, renderer, config);


        return handler.Execute(args);
    }
}
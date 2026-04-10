using TermNote.Commands;
using TermNote.Services;

var store = new NoteStore();
var handler = new CommandHandler(store);
return handler.Execute(args);
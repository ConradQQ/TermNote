using TermNote.Models;
namespace TermNote.Services;

public interface IStorageProvider
{
  void Save(List<Note> notes);
  List<Note> Load();

}
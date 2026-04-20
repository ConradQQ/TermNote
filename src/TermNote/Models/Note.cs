namespace TermNote.Models;

public class Note
{
  public string Id { get; init; }
  public string Content { get; set; }
  public Priority Priority { get; set; }
  public List<string> Tags { get; set; } = new List<string>();
  public DateTime CreatedAt { get; init; }

  public Note(string content, Priority priority = Priority.Medium, List<string>? tags = null)
  {

    if (string.IsNullOrWhiteSpace(content))
    { // Check if content is null, empty, or consists only of whitespace
      throw new ArgumentException("Content cannot be empty.");
    }

    Id = Guid.NewGuid().ToString("N")[..4];
    Content = content;
    CreatedAt = DateTime.UtcNow;
    Priority = priority;
    Tags = tags ?? [];
  }

  private Note()
  { // Parameterless constructor for deserialization -- needed for JSON deserialization 

    Id = string.Empty;
    Content = string.Empty;

  }
  public string GetAge()
  {
    var elapsed = DateTime.UtcNow - CreatedAt;

    return elapsed.TotalMinutes switch
    {
      < 1 => "Just now",
      < 60 => $"{(int)elapsed.TotalMinutes} minute(s) ago",
      < 1440 => $"{(int)elapsed.TotalHours} hour(s) ago",
      _ => $"{(int)elapsed.TotalDays} day(s) ago"
    };
  }
  public override string ToString()
  {

    return $"Note(Id: {Id}, Content: {Content}, CreatedAt: {CreatedAt})";

  }

}



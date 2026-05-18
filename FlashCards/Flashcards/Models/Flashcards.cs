namespace Flashcards.Models;

public class Flashcard
{
    public int Id { get; set; }
    public int StackId { get; set; }
    public string Front { get; set; } = string.Empty;
    public string Back { get; set; } = string.Empty;
}
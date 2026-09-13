namespace Flashcards.Models;

public class FlashcardDto
{
    public int DisplayID { get; set; }
    public int FlashcardsID { get; set; }
    public string Front { get; set; } = string.Empty;
    public string Back { get; set; } = string.Empty;
}

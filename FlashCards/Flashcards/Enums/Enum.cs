namespace Flashcards.Enums;

public class Enums
{
    internal enum MenuAction
    {
        ManageStacks,
        ManageFlashcards,
        StudySession,
        ChronoSession,
        Exit
    }

    internal enum StackAction
    {
        ViewStacks,
        CreateStack,
        UpdateStack,
        DeleteStack,
        BackToMainMenu
    }

    internal enum FlashcardAction
    {
        ViewFlashcards,
        CreateFlashcard,
        UpdateFlashcard,
        DeleteFlashcard,
        BackToMainMenu
    }
}
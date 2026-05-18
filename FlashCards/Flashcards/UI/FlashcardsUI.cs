using Spectre.Console;
using static Flashcards.Enums.Enums;

namespace Flashcards.UI;

internal class FlashcardsUI
{
    public void flashcardsMenu()
    {
        bool closeApp = false;
        while (!closeApp)
        {
            AnsiConsole.Clear();
            var actionChoice = AnsiConsole.Prompt(
            new SelectionPrompt<FlashcardAction>()
            .Title("What would you like to do?")
            .UseConverter(option => option switch
            {
                FlashcardAction.ViewFlashcards => "View Flashcards",
                FlashcardAction.CreateFlashcard => "Create Flashcard",
                FlashcardAction.UpdateFlashcard => "Update Flashcard",
                FlashcardAction.DeleteFlashcard => "Delete Flashcard",
                FlashcardAction.BackToMainMenu => "Back to Main Menu",
                _ => option.ToString()
            })
            .AddChoices(Enum.GetValues<FlashcardAction>()));

            switch (actionChoice)
            {
                case FlashcardAction.ViewFlashcards:
                    break;
                case FlashcardAction.CreateFlashcard:
                    break;
                case FlashcardAction.UpdateFlashcard:
                    break;
                case FlashcardAction.DeleteFlashcard:
                    break;
                case FlashcardAction.BackToMainMenu:
                    closeApp = true;
                    break;
            }
        }
    }
}
